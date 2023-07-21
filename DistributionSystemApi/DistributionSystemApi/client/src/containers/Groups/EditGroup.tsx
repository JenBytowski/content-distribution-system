import React, { useEffect, useState, ChangeEvent } from "react";
import Input from "../../UI/Inputs/Input";
import { Modal, Form, FormGroup, Container, Row, Toast } from "react-bootstrap";
import { returnInputGroupConfiguration } from "../../Utility/Group/InputGroupConfiguration";
import * as formUtilityActions from "../../Utility/Recipient/RecipientFormUtility";
import { AxiosResponse } from "axios";
import axios from "../../axios/axios";
import Button from "@mui/material/Button";
import Checkbox from "@mui/material/Checkbox";
import "../Modal.scss";
import { FormElement } from "../../components/Models/Form/FormElement";
import { GroupForm } from "../../components/Models/Form/GroupForm";
import { CreateGroupModel } from "../../components/Models/Group/CreateGroupModel";
import { Groups } from "../../components/Models/Group/Groups";
import Recipient from "../../components/Models/Recipients/Recipient";

interface EditGroupProps {
  show: boolean;
  onHide: () => void;
  groupId: string;
}

const EditGroup: React.FC<EditGroupProps> = ({ show, onHide, groupId }) => {
  const [isFormValid, setFormValid] = useState(false);
  const [formElementsArray, setFormElementsArray] = useState<FormElement[]>([]);
  const [groupForm, setGroupForm] = useState<GroupForm>({});
  const [recipients, setRecipients] = useState<Recipient[]>([]);
  const [selectedRecipients, setSelectedRecipients] = useState<Groups[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const recipientDataResponse = await axios.get("/api/Recipient");
        const groupsResponse = await axios.get(`/api/RecipientGroup/${groupId}`);
        const groupsData = groupsResponse.data;
        const recipientData = recipientDataResponse.data.items;
        const groupFormConfig = returnInputGroupConfiguration();
        const createGroupFormConfig = {
          ...groupFormConfig,
          title: {
            ...groupFormConfig.title,
            value: groupsData.title,
          },
        };

        setGroupForm(createGroupFormConfig);
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(createGroupFormConfig)
        );
        setRecipients(recipientData || []);
        setSelectedRecipients(groupsData.recipients || []);
      } catch (error) {
        console.error("Error fetching group data:", error);
      }
    };

    if (show && groupId) {
      fetchData().catch(console.error);
    }
  }, [show, groupId]);

  const handleChangeEvent = (
    event: ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    id: string
  ) => {
    const createdGroupForm = { ...groupForm };

      if (id === "recipients") {
      const selectElement = event.target as HTMLSelectElement;
      const selectedRecipients = Array.from(selectElement.selectedOptions, (option) => option.value);
      createdGroupForm[id].value = selectedRecipients;
    } else {
      createdGroupForm[id].value = event.target.value;
    }

    createdGroupForm[id] = formUtilityActions.executeValidationAndReturnFormElement(
      event,
      createdGroupForm,
      id
    );

    const counter = formUtilityActions.countInvalidElements(createdGroupForm);

    setFormElementsArray(
      formUtilityActions.convertStateToArrayOfFormObjects(createdGroupForm)
    );
    setGroupForm(createdGroupForm);
    setFormValid(counter === 0);

    const updatedElement = { ...createdGroupForm[id] };
    updatedElement.touched = true;
    createdGroupForm[id] = updatedElement;
  };

  const handleRecipientsCheckboxChange = (event: ChangeEvent<HTMLInputElement>, recipientId: string) => {
    setSelectedRecipients((prevSelectedRecipients) => {
      const isRecipientSelected = prevSelectedRecipients.some(
        (recipient) => recipient.recipientId === recipientId
      );
      const updatedRecipients = isRecipientSelected
        ? prevSelectedRecipients.filter((recipient) => recipient.recipientId !== recipientId)
        : [...prevSelectedRecipients, { groupId: groupId, recipientId: recipientId }];
      return updatedRecipients;
    });
  };

  const updateGroup = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const groupToUpdate: CreateGroupModel = {
      id: groupId,
      title: groupForm.title.value,
      recipientIds: selectedRecipients.map((recipient) => recipient.recipientId),
    };

    axios
      .put(`/api/RecipientGroup/${groupId}`, groupToUpdate)
      .then((response: AxiosResponse<any>) => {
        console.log("Group successfully updated", response.data);

        onHide();
      })
      .catch((error) => {
        console.error("Error updating group", error);
        if (error.response && error.response.data && error.response.data.message) {
          setErrorMessage(error.response.data.message);
        } else {
          setErrorMessage("An error occurred while updating");
        }
      });
  };

  return (
    <Modal show={show} onHide={onHide} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Editing Group</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Container>
          {errorMessage && (
            <Toast onClose={() => setErrorMessage(null)} delay={5000} autohide>
              <strong>Error</strong>
              <Toast.Body>{errorMessage}</Toast.Body>
            </Toast>
          )}
          <Form onSubmit={updateGroup}>
            {formElementsArray.map((element) => (
              <Input
                key={element.id}
                elementType={element.config.element}
                id={element.id}
                label={element.config.label}
                type={element.config.type}
                value={element.config.value}
                changed={(event: ChangeEvent<HTMLInputElement | HTMLSelectElement>) =>
                  handleChangeEvent(event, element.id)
                }
                errorMessage={element.config.errorMessage}
                invalid={!element.config.valid}
                shouldValidate={element.config.validation}
                touched={element.config.touched}
                blur={(event: ChangeEvent<HTMLInputElement | HTMLSelectElement>) =>
                  handleChangeEvent(event, element.id)
                }
                options={element.config.options}
              />
            ))}
            <br />
            <FormGroup>
              <Row>
                <Button type="submit" disabled={!isFormValid}>
                  Save
                </Button>
                <Button onClick={onHide}>Cancel</Button>
              </Row>
            </FormGroup>
          </Form>
        </Container>
      </Modal.Body>
      <Modal.Footer>
        <Container className="groups-container">
          <h5>Select Recipients</h5>
          {Array.isArray(recipients) && recipients.map((recipient: Recipient) => (
            <div className="checkbox" key={recipient.id}>
              <Checkbox
                checked={selectedRecipients.some((r) => r.recipientId === recipient.id)}
                onChange={(event) => handleRecipientsCheckboxChange(event, recipient.id)}
              />
              <div>{recipient.title}</div>
            </div>
          ))}
        </Container>
      </Modal.Footer>
    </Modal>
  );
};

export default EditGroup;