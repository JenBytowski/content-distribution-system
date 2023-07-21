export function returnInputGroupConfiguration() {
    return {
      title: {
        element: 'input',
        type: 'text',
        value: '',
        validation: { required: true },
        valid: false,
        touched: false,
        errorMessage: 'Field is empty',
        label: 'Title',
      },
      recipients: {
        element: '',
        validation: {
          required: false,
        },
        valid: true,
        touched: false,
        label: 'Recipients',
        multiple: true, 
        options: [],
      },
    };
  }