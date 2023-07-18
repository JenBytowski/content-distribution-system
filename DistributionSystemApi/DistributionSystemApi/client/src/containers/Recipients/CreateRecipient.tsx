import React, { useEffect, useState, ChangeEvent } from "react";
import Input from "../../UI/Inputs/Input";
import { Modal, Form, FormGroup, Container, Row } from "react-bootstrap";
import { returnInputRecipientConfiguration } from "../../Utility/Recipient/InputRecipientConfiguration";
import * as formUtilityActions from "../../Utility/Recipient/RecipientFormUtility";
import { AxiosResponse } from "axios";
import axios from "../../axios/axios";
import "../Modal.scss";
import Button from '@mui/material/Button';
import Checkbox from '@mui/material/Checkbox';
import { Group } from "../../components/Models/Group/Group";
import { FormElement } from "../../components/Models/Form/FormElement";
import { RecipientForm } from "../../components/Models/Form/RecipientForm";
import CreateRecipientModel from "../../components/Models/Recipients/CreateRecipientModel";
import { Groups } from "../../components/Models/Group/Groups";

interface CreateRecipientProps {
  show: boolean;
  onHide: () => void;
  onRecipientCreated: (recipient: CreateRecipientModel, selectedGroups: Groups[]) => void;
  availableGroups: Group[];
}

const CreateRecipient: React.FC<CreateRecipientProps> = ({
  show,
  onHide,
  onRecipientCreated,
  availableGroups,
}) => {
  const [isFormValid, setFormValid] = useState(false);
  const [formElementsArray, setFormElementsArray] = useState<FormElement[]>([]);
  const [recipientForm, setRecipientForm] = useState<RecipientForm>({});
  const [groups, setGroups] = useState<Group[]>(availableGroups);
  const [selectedGroups, setSelectedGroups] = useState<Groups[]>([]);

  const handleCreateRecipientCancel = () => {
    onHide();
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get("/api/RecipientGroup");
        const groupsData = response.data;
        const recipientFormConfig = returnInputRecipientConfiguration();
        const createRecipientFormConfig = {
          ...recipientFormConfig,
        };

        setRecipientForm(createRecipientFormConfig);
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(createRecipientFormConfig)
        );
        setGroups(groupsData || []);
      } catch (error) {
        console.error("Error fetching groups:", error);
      }
    };

    fetchData().catch(console.error);
  }, []);

  const handleGroupCheckboxChange = (event: ChangeEvent<HTMLInputElement>, groupId: string) => {
    setSelectedGroups((prevSelectedGroups) => {
      const isGroupSelected = prevSelectedGroups.some((group) => group.groupId === groupId);
      const updatedGroups = isGroupSelected
        ? prevSelectedGroups.filter((group) => group.groupId !== groupId)
        : [...prevSelectedGroups, { recipientId: '', groupId }];
      return updatedGroups;
    });
  };

  const handleChangeEvent = (
    event: ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    id: string
  ) => {
    const createdRecipientForm = { ...recipientForm };
  
    if (id === 'groups') {
      const selectElement = event.target as HTMLSelectElement;
      const selectedGroups = Array.from(selectElement.selectedOptions, (option) => option.value);
      createdRecipientForm[id].value = selectedGroups;
    } else {
      createdRecipientForm[id].value = event.target.value;
    }
  
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
      telephoneNumber: recipientForm.telephoneNumber.value !== "" ? recipientForm.telephoneNumber.value : null,
      groups: selectedGroups.length > 0 ? selectedGroups.map(group => group.groupId) : []
    };
  
    axios
      .post("/api/Recipient", recipientToCreate)
      .then((response: AxiosResponse<any>) => {
        console.log("Recipient successfully created", response.data);
  
        const createdRecipient = {
          ...recipientToCreate,
          id: response.data.id,
        };
  
        onRecipientCreated(createdRecipient, selectedGroups);
  
        setRecipientForm(returnInputRecipientConfiguration());
        setFormElementsArray(
          formUtilityActions.convertStateToArrayOfFormObjects(returnInputRecipientConfiguration())
        );
        setFormValid(false);
  
        handleCreateRecipientCancel();
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
                <Button onClick={handleCreateRecipientCancel}>
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

export default CreateRecipient;