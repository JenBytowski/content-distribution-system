export interface InputProps {
    elementType: string;
    id: string;
    label: string;
    type: string;
    value: any;
    options?: { value: string; displayValue: string }[];
    changed: (event: React.ChangeEvent<HTMLInputElement>) => void;
    blur: (event: React.FocusEvent<HTMLInputElement>) => void;
    invalid: boolean;
    shouldValidate: boolean;
    touched: boolean;
    errorMessage: string;
    multiple?: boolean;
  }