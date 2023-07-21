import { RecipientForm } from "../../components/Models/Form/RecipientForm";

export function convertStateToArrayOfFormObjects(formObject: RecipientForm) {
  const formElementsArray = [];
  for (let key in formObject) {
    const formElement = { id: key, config: formObject[key] };
    if (formObject[key].errorMessage) {
      formElement.config.errorMessage = formObject[key].errorMessage;
    }
    formElementsArray.push(formElement);
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

    if (validation.email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      const isValidEmail = emailRegex.test(value);
      validationObject.isValid = validationObject.isValid && isValidEmail;
      validationObject.errorMessage = isValidEmail
        ? ""
        : "Incorrect email format";
      if(value === "")
      value = "-";
    }

    if (validationObject.isValid && validation.phoneNumber) {
      if (value.trim() !== "") { 
        const phoneNumberRegex = /^\d{7,20}$/;
        validationObject.isValid = phoneNumberRegex.test(value);
        validationObject.errorMessage = "Incorrect phone number format";
      } else {
        validationObject.isValid = true;
        validationObject.errorMessage = "";
        value = "-";
      }
    }

    if (validationObject.isValid && validation.maxLength) {
      validationObject.isValid = value.length <= 60;
      validationObject.errorMessage = "Not allowed more than 60 characters";
    }

    return validationObject;
  } else {
    return validationObject;
  }
}

export function executeValidationAndReturnFormElement(
  event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
  updatedRecipientForm: RecipientForm,
  id: string
) {
  let formElement = { ...updatedRecipientForm[id] };
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

export function countInvalidElements(recipientForm: RecipientForm) {
  let countInvalidElements = 0;
  for (let element in recipientForm) {
    if (!recipientForm[element].valid) {
      countInvalidElements = countInvalidElements + 1;
      break;
    }
  }
  
  return countInvalidElements;
}