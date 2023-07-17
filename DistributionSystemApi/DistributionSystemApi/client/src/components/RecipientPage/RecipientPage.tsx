import { useEffect, useState } from "react";
import axios from "../../axios/axios";
import CreateRecipient from "../../containers/Recipients/CreateRecipient";
import Button from '@mui/material/Button';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import Table from '@mui/material/Table';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import DeleteRecipient from "../../containers/Recipients/DeleteRecipient";
import EditRecipient from "../../containers/Recipients/EditRecipient";
import Recipient from "../Models/Recipients/Recipient";
import { Group } from "../Models/Group/Group";

export default function RecipientPage() {
  const [data, setData] = useState<Recipient[]>([]);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editModalRecipient, setEditModalRecipient] = useState<Recipient | null>(null);
  const [deleteModalRecipient, setDeleteModalRecipient] = useState<Recipient | null>(null);
  const [groups, setGroups] = useState<Group[]>([]);

  const fetchData = async () => {
    try {
      const recipientResponse = await axios.get("/api/Recipient");
      const groupResponse = await axios.get("/api/RecipientGroup");
      setData(recipientResponse.data);
      setGroups(groupResponse.data);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  const handleEditModalOpen = (recipient: Recipient) => {
    setEditModalRecipient(recipient);
  };
  
  const handleDeleteModalOpen = (recipient: Recipient) => {
    setDeleteModalRecipient(recipient);
  };

  const handleEditModalClose = () => {
    setEditModalRecipient(null);
  };

  const handleDeleteModalClose = () => {
    setDeleteModalRecipient(null);
  };

  const handleCreateModalOpen = () => {
    setShowCreateModal(true);
  };

  const handleCreateModalClose = () => {
    setShowCreateModal(false);
  };

  useEffect(() => {
    fetchData().catch(console.error);
  }, []);

  const handleRecipientUpdated = (updatedRecipient: Recipient) => {
    console.log("Recipient updated:", updatedRecipient);
    fetchData();
  };

  const handleRecipientDeleted = () => {
    console.log("Recipient deleted");
    fetchData();
  };

  const handleCreateModalSave = (recipient: Recipient) => {
    const newRecipient = { ...recipient, group: null };
    setData((prevData) => [...prevData, newRecipient]);
    setShowCreateModal(false);
  };

  const getGroupNameById = (groupId: string | null | undefined) => {
    const group = groups.find((group) => String(groupId) === String(group.id) );
    return group ? group.title : "No Group";
  };

  return (
    <>
      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="recipient table">
          <TableHead>
            <TableRow>
              <TableCell align="center">Title</TableCell>
              <TableCell align="center">Email</TableCell>
              <TableCell align="center">Telephone Number</TableCell>
              <TableCell align="center">Group</TableCell>
              <TableCell>
                <EditIcon />
              </TableCell>
              <TableCell>
                <DeleteIcon />
              </TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {data.map((recipient) => (
              <TableRow key={recipient.id} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                <TableCell align="center">{recipient.title}</TableCell>
                <TableCell align="center">{recipient.email}</TableCell>
                <TableCell align="center">{recipient.telephoneNumber || "-"}</TableCell>
                <TableCell align="center">
                  {groups.length > 0 ? getGroupNameById(recipient.groupId) : "Loading..."}
                </TableCell>
                <TableCell>
              <EditIcon aria-label="edit" onClick={() => handleEditModalOpen(recipient)} />
            </TableCell>
            <TableCell>
              <DeleteIcon aria-label="delete" onClick={() => handleDeleteModalOpen(recipient)} />
            </TableCell>
            {editModalRecipient && (
              <EditRecipient
                show={editModalRecipient === recipient}
                onHide={handleEditModalClose}
                onRecipientUpdated={handleRecipientUpdated}
                recipient={recipient}
              />
            )}
            {deleteModalRecipient && (
              <DeleteRecipient
                show={deleteModalRecipient === recipient}
                onHide={handleDeleteModalClose}
                onDelete={handleRecipientDeleted}
                recipientId={recipient.id}
              />
            )}
          </TableRow>
        ))}
      </TableBody>
        </Table>
      </TableContainer>
  
      <Button className="custom-btn add" onClick={handleCreateModalOpen}>
        <span>Add recipient</span>
      </Button>
  
      <CreateRecipient
        show={showCreateModal}
        onHide={handleCreateModalClose}
        onRecipientCreated={handleCreateModalSave}
      />
    </>
  );
}