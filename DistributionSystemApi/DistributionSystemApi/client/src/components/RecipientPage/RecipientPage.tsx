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
import {TablePagination} from "@mui/material";
import { TableFooter } from "@mui/material";
import Paper from '@mui/material/Paper';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import DeleteRecipient from "../../containers/Recipients/DeleteRecipient";
import EditRecipient from "../../containers/Recipients/EditRecipient";
import Recipient from "../Models/Recipients/Recipient";
import CreateRecipientModel from "../Models/Recipients/CreateRecipientModel";
import { Group } from "../Models/Group/Group";
import { Groups } from "../Models/Group/Groups";

export default function RecipientPage() {
  const [data, setData] = useState<Recipient[]>([]);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editModalRecipientId, setEditModalRecipientId] = useState<string | null>(null);
  const [deleteModalRecipient, setDeleteModalRecipient] = useState<Recipient | null>(null);
  const [allGroups, setGroups] = useState<Group[]>([]);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [groupNames, setGroupNames] = useState<string[]>([]);
  
  const fetchData = async () => {
    try {
      const response = await axios.get(`/api/Recipient?page=${page}&pageSize=${pageSize}`);
      const groupResponse = await axios.get("/api/RecipientGroup");
      setData(response.data.items);
      setGroups(groupResponse.data);
      setTotalCount(response.data.totalCount);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  useEffect(() => {
    setGroupNames(data.map((recipient) => getGroupNamesByIds(recipient.groups, allGroups)));
  }, [data, allGroups]);

  const handleEditModalOpen = (recipientId: string) => {
    setEditModalRecipientId(recipientId);
  };
  
  const handleDeleteModalOpen = (recipient: Recipient) => {
    setDeleteModalRecipient(recipient);
  };

  const handleEditModalClose = () => {
    setEditModalRecipientId(null);
    fetchData();
  };

  const handleDeleteModalClose = () => {
    setDeleteModalRecipient(null);
    fetchData();
  };

  const handleCreateModalOpen = () => {
    setShowCreateModal(true);
    fetchData();
  };

  const handleCreateModalClose = () => {
    setShowCreateModal(false);
    fetchData();
  };
  const handlePageChange = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
    setPage(newPage);
  };
  
  const handlePageSizeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPageSize(Number(event.target.value));
    setPage(1);
  };
  useEffect(() => {
    fetchData().catch(console.error);
  }, []);

  const handleCreateModalSave = (recipient: CreateRecipientModel, selectedGroups: Groups[]) => {
    const newGroups: Groups[] = selectedGroups.map((groupId) => ({
      recipientId: String(recipient.id),
      groupId: String(groupId),
    }));
  
    const newRecipient: Recipient = {
      id: recipient.id,
      title: recipient.title,
      email: recipient.email,
      telephoneNumber: recipient.telephoneNumber,
      groups: newGroups,
    };

    const newTotalPages = Math.ceil((totalCount + 1) / pageSize);

  if (page !== newTotalPages) {
    setPage(newTotalPages);
  }
  
    setData((prevData: Recipient[]) => [...prevData, newRecipient]);
    setShowCreateModal(false);
  };

  const getGroupNamesByIds = (groups: Groups[], allGroups: Group[]) => {
    if (allGroups.length === 0) {
      return "No Group";
    }
  
    const groupNames = groups
      .map((group) => {
        const foundGroup = allGroups?.find((item) => item.id === group.groupId);
        return foundGroup ? foundGroup.title : null;
      })
      .filter(Boolean);
  
    if (groupNames.length === 0) {
      return "No Group";
    }
  
    return groupNames.join(", ");
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
              <TableCell align="center">Groups</TableCell>
              <TableCell align="center">EditGroup</TableCell>
              <TableCell align="center">DeleteGroup</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
          {data.map((recipient, index) => (
              <TableRow key={recipient.id} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                <TableCell align="center">{recipient.title}</TableCell>
                <TableCell align="center">{recipient.email}</TableCell>
                <TableCell align="center">{recipient.telephoneNumber == null ? "-" : recipient.telephoneNumber}</TableCell>
                <TableCell align="center">{allGroups.length > 0 ? groupNames[index] : "No Group"}</TableCell>
                <TableCell  align="center">
                  <EditIcon aria-label="edit" onClick={() => handleEditModalOpen(recipient.id)} />
                </TableCell>
                <TableCell  align="center">
                  <DeleteIcon aria-label="delete" onClick={() => handleDeleteModalOpen(recipient)} />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
          <TableFooter>
  <TableRow>
    <TablePagination
      rowsPerPageOptions={[5, 10, 25]}
      colSpan={6}
      count={totalCount}
      rowsPerPage={pageSize}
      page={page - 1}
      onPageChange={handlePageChange}
      onRowsPerPageChange={handlePageSizeChange}
    />
  </TableRow>
</TableFooter>
        </Table>
      </TableContainer>
  
      <Button className="custom-btn add" onClick={handleCreateModalOpen}>
        <span>Add recipient</span>
      </Button>

      {editModalRecipientId && (
        <EditRecipient
          show={Boolean(editModalRecipientId)}
          onHide={handleEditModalClose}
          recipientId={editModalRecipientId}
        />
      )}

      {deleteModalRecipient && (
        <DeleteRecipient
          show={Boolean(deleteModalRecipient)}
          onHide={handleDeleteModalClose}
          recipientId={deleteModalRecipient.id}
        />
      )}
      
      {showCreateModal && (
        <CreateRecipient
        show={Boolean(showCreateModal)}
        onHide={handleCreateModalClose}
        onRecipientCreated={handleCreateModalSave}
        availableGroups={allGroups}
/>
      )}
    </>
  );
}