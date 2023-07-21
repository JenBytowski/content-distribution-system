import React from "react";
import { Modal } from "react-bootstrap";
import axios from "../../axios/axios";
import "../Modal.scss";
import Button from '@mui/material/Button';

interface DeleteRecipientProps {
  show: boolean;
  onHide: () => void;
  recipientId: string;
}

const DeleteRecipient: React.FC<DeleteRecipientProps> = ({
  show,
  onHide,
  recipientId,
}) => {
  const handleDelete = () => {
    axios
      .delete(`/api/Recipient/${recipientId}`)
      .then(() => {
        console.log("Recipient successfully deleted");
        onHide();
      })
      .catch((error) => {
        console.error("Error deleting recipient", error);
      });
  };

  return (
    <Modal show={show} onHide={onHide} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Deleting a recipient</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>Are you sure you want to delete the recipient?</p>
      </Modal.Body>
      <Modal.Footer>
        <Button onClick={onHide}>
          Cancel
        </Button>
        <Button onClick={handleDelete}>
          Delete
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default DeleteRecipient;