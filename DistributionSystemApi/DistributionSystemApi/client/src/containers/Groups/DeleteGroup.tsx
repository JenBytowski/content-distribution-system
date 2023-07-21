import React from "react";
import { Modal } from "react-bootstrap";
import axios from "../../axios/axios";
import "../Modal.scss";
import Button from '@mui/material/Button';

interface DeleteGroupProps {
  show: boolean;
  onHide: () => void;
  groupId: string;
}

const DeleteRecipient: React.FC<DeleteGroupProps> = ({
  show,
  onHide,
  groupId: groupId,
}) => {
  const handleDelete = () => {
    axios
      .delete(`/api/RecipientGroup/${groupId}`)
      .then(() => {
        console.log("Group successfully deleted");
        onHide();
      })
      .catch((error) => {
        console.error("Error deleting group", error);
      });
  };

  return (
    <Modal show={show} onHide={onHide} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Deleting a group</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>Are you sure you want to delete the group?</p>
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