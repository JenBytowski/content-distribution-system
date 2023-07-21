import React from "react";
import { Modal } from "react-bootstrap";
import { Group } from "../../components/Models/Group/Group";
import Recipient from "../../components/Models/Recipients/Recipient";
import Button from "@mui/material/Button";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";

interface GroupInfoProps {
  group: Group;
  recipients: Recipient[];
  open: boolean;
  onClose: () => void;
}

const GroupDetailsModal: React.FC<GroupInfoProps> = ({ group, recipients, open, onClose }) => {
  return (
    <Modal show={open} onHide={onClose} contentClassName="popup-modal">
      <Modal.Header>
        <Modal.Title>Group Info: Recipients in {group.title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <TableContainer component={Paper}>
          <Table aria-label="group details table">
            <TableHead>
              <TableRow>
                <TableCell>Title</TableCell>
                <TableCell>Email</TableCell>
                <TableCell>Telephone Number</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {recipients.length > 0 ? (
                recipients.map((recipient) => (
                  <TableRow key={recipient.id}>
                    <TableCell>{recipient.title}</TableCell>
                    <TableCell>{recipient.email}</TableCell>
                    <TableCell>
                      {recipient.telephoneNumber ? recipient.telephoneNumber : "-"}
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={3}>No Recipients</TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Modal.Body>
      <Modal.Footer>
        <Button onClick={onClose}>Close</Button>
      </Modal.Footer>
    </Modal>
  );
};

export default GroupDetailsModal;