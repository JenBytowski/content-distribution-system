import React, { useEffect, useState, ChangeEvent } from "react";
import Input from "../../UI/Inputs/Input";
import { Modal, Form, FormGroup, Container, Row, Toast } from "react-bootstrap";
import { returnInputRecipientConfiguration } from "../../Utility/Recipient/InputRecipientConfiguration";
import * as formUtilityActions from "../../Utility/Recipient/RecipientFormUtility";
import { AxiosResponse } from "axios";
import axios from "../../axios/axios";
import Button from '@mui/material/Button';
import "../Modal.scss";
import { Checkbox } from "@mui/material";
import { Group } from "../../components/Models/Group/Group";
import { FormElement } from "../../components/Models/Form/FormElement";
import { RecipientForm } from "../../components/Models/Form/RecipientForm";
import { Groups } from "../../components/Models/Group/Groups";

interface EditRecipientProps {
  show: boolean;
  onHide: () => void;
  recipientId: string;
}

const EditRecipient: React.FC<EditRecipientProps> = ({
  show,
  onHide,
  recipientId,
}) => {
  const [isFormValid, setFormValid] = useState(false);
  const [formElementsArray, setFormElementsArray] = useState<FormElement[]>([]);
  const [recipientForm, setRecipientForm] = useState<RecipientForm>({});
  const [groups, setGroups] = useState<Group[]>([]); 
  const [selectedGroups, setSelectedGroups] = useState<Groups[]>([]); 
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
 
  useEffect(() => {
    const fetchRecipientData = async () => {
      try {
        const groupsResponse = await axios.get("/api/RecipientGroup");
        const recipientDataResponse = await axios.get(`/api/Recipient/${recipientId}`);

        const groupsData = groupsResponse.data;
        const recipientData = recipientDataResponse.data;

        const recipientFormConfig = returnInputRecipientConfiguration();
        const updatedRecipientFormConfig = {
          ...recipientFormConfig,
          title: {
            ...recipientFormConfig.title,
            value: recipientData.title,
          },
          email: {
            ...recipientFormConfig.email,
            value: recipientData.email,
          },
          telephoneNumber: {
            ...recipientFormConfig.telephoneNumber,
            value: recipientData.telephoneNumber !== null ? recipientData.telephoneNumber : "",
          },
        };

        setRecipientForm(updatedRecipientFormConfig);
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(updatedRecipientFormConfig)
        );
        setGroups(groupsData || []);
        setSelectedGroups(recipientData.groups || []);
      } catch (error) {
        console.error("Error fetching recipient data:", error);
      }
    };

    if (show && recipientId) {
      fetchRecipientData().catch(console.error);
    }
  }, [show, recipientId]);

  const handleChangeEvent = (
    event: ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    id: string
  ) => {
    const createdRecipientForm = { ...recipientForm };
    createdRecipientForm[id] = formUtilityActions.executeValidationAndReturnFormElement(
      event,
      createdRecipientForm,
      id
    );

    const counter = formUtilityActions.countInvalidElements(createdRecipientForm);

    setFormElementsArray(
      formUtilityActions.convertStateToArrayOfFormObjects(createdRecipientForm)
    );
    setRecipientForm(createdRecipientForm);
    setFormValid(counter === 0);

    const updatedElement = { ...createdRecipientForm[id] };
    updatedElement.touched = true;
    createdRecipientForm[id] = updatedElement;
  };

  const handleGroupCheckboxChange = (event: ChangeEvent<HTMLInputElement>, groupId: string) => {
    setSelectedGroups((prevSelectedGroups) => {
      const isGroupSelected = prevSelectedGroups.some((group) => group.groupId === groupId);
      const updatedGroups = isGroupSelected
        ? prevSelectedGroups.filter((group) => group.groupId !== groupId)
        : [...prevSelectedGroups, { groupId }];
      return updatedGroups;
    });
  };

  const updateRecipient = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const recipientToUpdate = {
      id: recipientId,
      title: recipientForm.title.value,
      email: recipientForm.email.value,
      telephoneNumber: recipientForm.telephoneNumber.value !== "" ? recipientForm.telephoneNumber.value : null,
      groups: selectedGroups.map((group) => group.groupId),
    };

    axios
      .put(`/api/Recipient/${recipientId}`, recipientToUpdate)
      .then((response: AxiosResponse<any>) => {
        console.log("Recipient successfully updated", response.data);

        onHide();
      })
      .catch((error) => {
        console.error("Error updating recipient", error);
        if (error.response && error.response.data && error.response.data.message) {
          setErrorMessage(errorMessage); 
        } else {
          setErrorMessage("An error occurred while updating");
        }
      });
  };

  return (
    <Modal show={show} onHide={onHide} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Editing Recipient</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Container>
        {errorMessage && (
            <Toast onClose={() => setErrorMessage(null)} delay={5000} autohide>
                <strong className="me-auto">Error</strong>
              <Toast.Body>{errorMessage}</Toast.Body>
            </Toast>
          )}
          <Form onSubmit={updateRecipient}>
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
                <Button onClick={onHide}>
                  Cancel
                </Button>
              </Row>
            </FormGroup>
          </Form>
        </Container>
      </Modal.Body>
      <Modal.Footer>
        <Container className = "groups-container">
          <h5>Select Groups</h5>
          {groups.map((group: Group) => (
            <div key={group.id}>
              <Checkbox
                checked={selectedGroups.some((g) => g.groupId === group.id)}
                onChange={(event) => handleGroupCheckboxChange(event, group.id)}
              />
              {group.title}
            </div>
          ))}
        </Container>
      </Modal.Footer>
    </Modal>
  );
};

export default EditRecipient;