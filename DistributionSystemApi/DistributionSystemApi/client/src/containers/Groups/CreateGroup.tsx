import React, { useEffect, useState, ChangeEvent } from "react";
import Input from "../../UI/Inputs/Input";
import { Modal, Form, FormGroup, Container, Row, Toast } from "react-bootstrap";
import { returnInputGroupConfiguration } from "../../Utility/Group/InputGroupConfiguration";
import * as formUtilityActions from "../../Utility/Recipient/RecipientFormUtility";
import { AxiosResponse } from "axios";
import axios from "../../axios/axios";
import "../Modal.scss";
import Button from "@mui/material/Button";
import Checkbox from "@mui/material/Checkbox";
import { FormElement } from "../../components/Models/Form/FormElement";
import { GroupForm } from "../../components/Models/Form/GroupForm";
import { CreateGroupModel } from "../../components/Models/Group/CreateGroupModel";
import { Groups } from "../../components/Models/Group/Groups";
import Recipient from "../../components/Models/Recipients/Recipient";

interface CreateGroupProps {
  show: boolean;
  onHide: () => void;
  onGroupCreated: (recipient: CreateGroupModel, selectedRecipients: Groups[]) => void;
  availableRecipients: Recipient[];
}

const CreateGroup: React.FC<CreateGroupProps> = ({
  show,
  onHide,
  onGroupCreated,
  availableRecipients,
}) => {
  const [isFormValid, setFormValid] = useState(false);
  const [formElementsArray, setFormElementsArray] = useState<FormElement[]>([]);
  const [groupForm, setGroupForm] = useState<GroupForm>({});
  const [recipients, setRecipients] = useState<Recipient[]>(availableRecipients);
  const [selectedRecipients, setSelectedRecipients] = useState<Groups[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const handleCreateRecipientCancel = () => {
    onHide();
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get("/api/Recipient");
        const recipientsData = response.data.items;
        const groupFormConfig = returnInputGroupConfiguration();
        const createGroupFormConfig = {
          ...groupFormConfig,
        };

        setGroupForm(createGroupFormConfig);
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(createGroupFormConfig)
        );
        setRecipients(recipientsData || []);
      } catch (error) {
        console.error("Error fetching recipients:", error);
      }
    };

    fetchData().catch(console.error);
  }, []);

  const handleRecipientsCheckboxChange = (event: ChangeEvent<HTMLInputElement>, recipientId: string) => {
    setSelectedRecipients((prevSelectedRecipients) => {
      const isRecipientSelected = prevSelectedRecipients.some((recipient) => recipient.recipientId === recipientId);
      const updatedRecipients = isRecipientSelected
        ? prevSelectedRecipients.filter((group) => group.recipientId !== recipientId)
        : [...prevSelectedRecipients, { groupId: "", recipientId: recipientId }];
      return updatedRecipients;
    });
  };

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
  };

  const handleCreateRecipient = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const groupToCreate = {
      title: groupForm.title.value,
      recipientIds : selectedRecipients.map((group) => group.recipientId),
    };
    
    axios
      .post("/api/RecipientGroup", groupToCreate)
      .then((response: AxiosResponse<any>) => {
  
        const createdGroup = {
          ...groupToCreate,
          id: response.data.id,
        };
        const groupId = response.data.id;

        const updatedSelectedRecipients = selectedRecipients.map((recipient) => ({
          groupId: groupId,
          recipientId: recipient.recipientId,
        }));
        
        onGroupCreated(createdGroup, updatedSelectedRecipients);
        setGroupForm(returnInputGroupConfiguration());
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(returnInputGroupConfiguration())
        );
        setFormValid(false);
  
        handleCreateRecipientCancel();
      })
      .catch((error) => {
        console.error("Error creating group", error);
        if (error.response && error.response.data && error.response.data.message) {
          setErrorMessage(error.response.data.message);
        } else {
          setErrorMessage("An error occurred while creating");
        }
      });
  };

  return (
    <Modal show={show} onHide={onHide} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Adding a group</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Container>
          {errorMessage && (
            <Toast onClose={() => setErrorMessage(null)} delay={5000} autohide>
                <strong>Error</strong>
              <Toast.Body>{errorMessage}</Toast.Body>
            </Toast>
          )}
          <Form onSubmit={handleCreateRecipient}>
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
                  Add
                </Button>
                <Button onClick={handleCreateRecipientCancel}>
                  Cancel
                </Button>
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
                checked={selectedRecipients.some((g) => g.recipientId === recipient.id)}
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

export default CreateGroup;