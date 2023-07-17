import React, { useEffect, useState, ChangeEvent } from "react";
import Input from "../../UI/Inputs/Input";
import { Form, FormGroup, Col, Container, Row, Modal } from "react-bootstrap";
import { returnInputRecipientConfiguration } from "../../Utility/Recipient/InputRecipientConfiguration";
import * as formUtilityActions from "../../Utility/Recipient/RecipientFormUtility";
import { AxiosResponse } from "axios";
import axios from "../../axios/axios";
import Button from '@mui/material/Button';
import "../Modal.scss";
import Recipient from "../../components/Models/Recipients/Recipient";
import { Group } from "../../components/Models/Group/Group";
import { FormElement } from "../../components/Models/Form/FormElement";
import { RecipientForm } from "../../components/Models/Form/RecipientForm";

interface EditRecipientProps {
  show: boolean;
  onHide: () => void;
  onRecipientUpdated: (recipient: Recipient) => void;
  recipient: Recipient;
}

const EditRecipient: React.FC<EditRecipientProps> = ({
  show,
  onHide,
  onRecipientUpdated,
  recipient,
}) => {
  const [isFormValid, setFormValid] = useState(false);
  const [formElementsArray, setFormElementsArray] = useState<FormElement[]>([]);
  const [recipientForm, setRecipientForm] = useState<RecipientForm>({});

  useEffect(() => {
    const recipientFormConfig = returnInputRecipientConfiguration();
    setRecipientForm(recipientFormConfig);
    setFormElementsArray(
      formUtilityActions.convertStateToArrayOfFormObjects(recipientFormConfig)
    );
  
    const fetchRecipientGroups = async () => {
      try {
        const response = await axios.get("/api/RecipientGroup");
        const groups = response.data;
        const updatedRecipientFormConfig = {
          ...recipientFormConfig,
          title: {
            ...recipientFormConfig.title,
            value: recipient.title,
          },
          email: {
            ...recipientFormConfig.email,
            value: recipient.email,
          },
          telephoneNumber: {
            ...recipientFormConfig.telephoneNumber,
            value: recipient.telephoneNumber || "",
          },
          groupId: {
            ...recipientFormConfig.groupId,
            value: recipient.groupId || "",
            options: [
              { value: "", displayValue: "No Group" },
              ...groups.map((group: Group) => ({
                value: group.id,
                displayValue: group.title,
                selected: group.id === recipient.groupId,
              })),
            ],
          },
        };
  
        setRecipientForm(updatedRecipientFormConfig);
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(updatedRecipientFormConfig)
        );
      } catch (error) {
        console.error("Error fetching recipient groups:", error);
      }
    };
    fetchRecipientGroups().catch(console.error);
  }, [recipient]);

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

  const updateRecipient = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const recipientToUpdate = {
      id: recipient.id,
      title: recipientForm.title.value,
      email: recipientForm.email.value,
      telephoneNumber: recipientForm.telephoneNumber.value,
      groupId: recipientForm.groupId.value === "" ? null : recipientForm.groupId.value,
    };

    axios
      .put(`/api/Recipient/${recipient.id}`, recipientToUpdate)
      .then((response: AxiosResponse<any>) => {
        console.log("Recipient successfully edit", response.data);

        onRecipientUpdated(response.data);

        onHide();
      })
      .catch((error) => {
        console.error("Error updating recipient", error);
      });
  };

  return (
    <Modal show={show} onHide={onHide} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Editing Recipient</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Container>
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
    </Modal>
  );
};

export default EditRecipient;