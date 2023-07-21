import { GroupForm } from "../../components/Models/Form/GroupForm";

export function convertStateToArrayOfFormObjects(formObject: GroupForm) {
    const formElementsArray = [];
    for (let key in formObject) {
      formElementsArray.push({
        id: key,
        config: formObject[key],
      });
    }
  
    return formElementsArray;
  }
  
  function checkValidity(value: string, validation: any) {
    let validationObject = {
      isValid: true,
      errorMessage: "",
    };
  
    if (validation) {
      if (validation.required) {
        validationObject.isValid = value.trim() !== "";
        validationObject.errorMessage = validationObject.isValid
          ? ""
          : "Field is required";
      }
  
      return validationObject;
    } else {
      return validationObject;
    }
  }
  
  export function executeValidationAndReturnFormElement(
    event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    updatedGroupForm: GroupForm,
    id: string
  ) {
    let formElement = { ...updatedGroupForm[id] };
    formElement.value = id === "" ? event : event.target.value;
    formElement.touched = true;
  
    const validationResponse = checkValidity(
      formElement.value,
      formElement.validation
    );
  
    formElement.valid = validationResponse.isValid;
    formElement.errorMessage = validationResponse.errorMessage;
  
    return formElement;
  }
  
  export function countInvalidElements(groupForm: GroupForm) {
    let countInvalidElements = 0;
    for (let element in groupForm) {
      if (!groupForm[element].valid) {
        countInvalidElements = countInvalidElements + 1;
        break;
      }
    }
    
    return countInvalidElements;
  }
