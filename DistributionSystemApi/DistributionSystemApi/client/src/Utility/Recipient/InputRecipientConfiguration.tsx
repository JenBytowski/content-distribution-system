export function returnInputRecipientConfiguration() {
  return {
    title: {
      element: "input",
      type: "text",
      value: "",
      validation: { required: true },
      valid: false,
      touched: false,
      errorMessage: "Field is empty",
      label: "Title",
    },
    email: {
      element: "input",
      type: "text",
      value: "",
      validation: {
        required: true,
        email: true,
      },
      valid: false,
      touched: false,
      errorMessage: "Incorrect email format",
      label: "Email",
    },
    telephoneNumber: {
      element: "input",
      type: "text",
      value: "",
      validation: {
        required: false,
        phoneNumber: true,
      },
      valid: true,
      touched: false,
      label: "Telephone number",
    },
    groups: {
      type: "select",
      value: [],
      validation: {
        required: false,
      },
      valid: true,
      touched: false,
      label: "Groups",
      multiple: true, 
      options: [],
    },
  };
}