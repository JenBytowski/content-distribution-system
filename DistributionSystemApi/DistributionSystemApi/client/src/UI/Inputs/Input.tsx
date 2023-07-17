import React from "react";
import {
  Container,
  FormGroup,
  Col,
  FormControl,
  FormLabel,
} from "react-bootstrap";
import "./Input.scss";
import Checkbox from '@mui/material/Checkbox';
import { InputProps } from "../../components/Models/Form/InputForms";

const Input: React.FC<InputProps> = (props) => {
  let inputField = null;
  let errorMessage = null;

  if (props.invalid && props.shouldValidate && props.touched) {
    errorMessage = <em>{props.errorMessage}</em>;
  }

  switch (props.elementType) {
    case "input":
      inputField = (
        <FormGroup controlId={props.id}>
          <Col componentclass={FormLabel} sm={2}>
            {props.label}
          </Col>
          <Container className="input">
            <Col sm={6}>
              <FormControl
                type={props.type}
                value={props.value}
                onChange={props.changed}
                onBlur={props.blur}
              />
            </Col>
          </Container>
          <Col>
            <em>{errorMessage}</em>
          </Col>
        </FormGroup>
      );
      break;
    case "select":
      inputField = (
        <FormGroup controlId={props.id}>
          <Col componentclass={FormLabel} sm={2}>
            {props.label}
          </Col>
          <Container className="input">
            <Col sm={6}>
              <FormControl
                as="select"
                value={props.value}
                onChange={props.changed}
                onBlur={props.blur}
              >
                {props.options && props.options.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.displayValue}
                  </option>
                ))}
              </FormControl>
            </Col>
          </Container>
          <Col>
            <em>{errorMessage}</em>
          </Col>
        </FormGroup>
      );
      break;
    case "checkbox":
      inputField = (
        <FormGroup controlId={props.id}>
          <Col componentclass={FormLabel} sm={2}>
            {props.label}
          </Col>
          <Container className="input">
            <Col sm={6}>
              <Checkbox
                checked={props.value}
                onChange={props.changed}
              />
            </Col>
          </Container>
          <Col>
            <em>{errorMessage}</em>
          </Col>
        </FormGroup>
      );
      break;
    default:
      inputField = null;
  }

  return <>{inputField}</>;
};

export default Input;