import React, { useEffect, useState, ChangeEvent } from "react";
import Input from "../../UI/Inputs/Input";
import { Modal, Form, FormGroup, Container, Row } from "react-bootstrap";
import { returnInputRecipientConfiguration } from "../../Utility/Recipient/InputRecipientConfiguration";
import * as formUtilityActions from "../../Utility/Recipient/RecipientFormUtility";
import { AxiosResponse } from "axios";
import axios from "../../axios/axios";
import "../Modal.scss";
import Button from '@mui/material/Button';
import { Group } from "../../components/Models/Group/Group";
import { FormElement } from "../../components/Models/Form/FormElement";
import { RecipientForm } from "../../components/Models/Form/RecipientForm";
import Recipient from "../../components/Models/Recipients/Recipient";

interface CreateRecipientProps {
  show: boolean;
  onHide: () => void;
  onRecipientCreated: (recipient: Recipient) => void;
}

const CreateRecipient: React.FC<CreateRecipientProps> = ({
  show,
  onHide,
  onRecipientCreated,
}) => {
  const [isFormValid, setFormValid] = useState(false);
  const [formElementsArray, setFormElementsArray] = useState<FormElement[]>([]);
  const [recipientForm, setRecipientForm] = useState<RecipientForm>({});

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get("/api/RecipientGroup");
        const groups = response.data;
  
        const recipientFormConfig = returnInputRecipientConfiguration();
        const createRecipientFormConfig = {
          ...recipientFormConfig,
          groupId: {
            ...recipientFormConfig.groupId,
            options: [
              { value: "", displayValue: "No Group" },
              ...groups.map((group: Group) => ({
                value: group.id,
                displayValue: group.title,
              })),
            ],
          },
        };
  
        setRecipientForm(createRecipientFormConfig);
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(createRecipientFormConfig)
        );
      } catch (error) {
        console.error("Error fetching groups:", error);
      }
    };
  
    fetchData().catch(console.error);
  }, []);

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
  };

  const handleCreateRecipient = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const recipientToCreate = {
      title: recipientForm.title.value,
      email: recipientForm.email.value,
      telephoneNumber: recipientForm.telephoneNumber.value,
      groupId: recipientForm.groupId.value === "" ? null : recipientForm.groupId.value,
    };

    axios
      .post("/api/Recipient", recipientToCreate)
      .then((response: AxiosResponse<any>) => {
        console.log("Recipient successfully created", response.data);

        onRecipientCreated(response.data);

        setRecipientForm(returnInputRecipientConfiguration());
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(
            returnInputRecipientConfiguration()
          )
        );
        setFormValid(false);

        onHide();
      })
      .catch((error) => {
        console.error("Error creating recipient", error);
      });
  };

  return (
    <Modal show={show} onHide={onHide} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Adding a recipient</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Container>
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

export default CreateRecipient;