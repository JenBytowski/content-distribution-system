import { useEffect, useState } from "react";
import axios from "../../axios/axios";
import { CreateGroupModel } from "../Models/Group/CreateGroupModel";
import Button from "@mui/material/Button";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import Table from "@mui/material/Table";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import DeleteIcon from "@material-ui/icons/DeleteSharp";
import InfoIcon from '@material-ui/icons/Info';
import EditIcon from "@material-ui/icons/Edit";
import DeleteGroup from "../../containers/Groups/DeleteGroup";
import EditGroup from "../../containers/Groups/EditGroup";
import { Group } from "../Models/Group/Group";
import { Groups } from "../Models/Group/Groups";
import Recipient from "../Models/Recipients/Recipient";
import CreateGroup from "../../containers/Groups/CreateGroup";
import GroupDetailsModal from "../../containers/Groups/GroupDetails";

export default function GroupPage() {
  const [data, setData] = useState<Group[]>([]);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editModalGroupId, setEditModalGroupId] = useState<string | null>(null);
  const [deleteModalGroup, setDeleteModalGroup] = useState<Group | null>(null);
  const [recipients, setRecipients] = useState<Recipient[]>([]);
  const [, setRecipientsNames] = useState<string[]>([]);
  const [showGroupDetailsModal, setShowGroupDetailsModal] = useState(false);
  const [selectedGroup, setSelectedGroup] = useState<Group | null>(null);

  const fetchData = async () => {
    try {
      const response = await axios.get(`/api/RecipientGroup`);
      const recipientResponse = await axios.get("/api/Recipient");
      setData(response.data);
      setRecipients(recipientResponse.data.items);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  useEffect(() => {
    if (data && recipients) {
      fetchData();
      setRecipientsNames(data.map((group) => getRecipientTitlesByIds(group.recipients, recipients)));
    }
  }, [data, recipients]);

  const handleGroupDetailsModalOpen = (group: Group) => {
    setSelectedGroup(group);
    setShowGroupDetailsModal(true);
  };

  const handleGroupDetailsModalClose = () => {
    setSelectedGroup(null);
    setShowGroupDetailsModal(false);
  };

  const handleEditModalOpen = (groupId: string) => {
    setEditModalGroupId(groupId);
  };

  const handleDeleteModalOpen = (group: Group) => {
    setDeleteModalGroup(group);
  };

  const handleEditModalClose = () => {
    setEditModalGroupId(null);
    fetchData();
  };

  const handleDeleteModalClose = () => {
    setDeleteModalGroup(null);
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

  const handleCreateModalSave = (group: CreateGroupModel, selectedRecipients: Groups[]) => {
    const newRecipients: Groups[] = selectedRecipients.map((recipientId) => ({
      groupId: String(group.id),
      recipientId: String(recipientId),
    }));

    const newGroup: Group = {
      id: group.id,
      title: group.title,
      recipients: newRecipients,
    };

    setData((prevData: Group[]) => [...prevData, newGroup]);
    setShowCreateModal(false);
  };

  const getRecipientTitlesByIds = (recipients: Groups[], allRecipients: Recipient[]) => {
    if (allRecipients.length === 0) {
      return "No Recipient";
    }

    const recipientsNames = recipients
      .map((recipient) => {
        const foundRecipient = allRecipients.find((item) => item.id === recipient.recipientId);
        return foundRecipient ? foundRecipient.title : null;
      })
      .filter(Boolean);

    if (recipientsNames.length === 0) {
      return "No Recipient";
    }

    return recipientsNames.join(", ");
  };

  return (
    <>
      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="group table">
          <TableHead>
            <TableRow>
              <TableCell align="center">Title</TableCell>
              <TableCell align="center">Recipients</TableCell>
              <TableCell align="center">Info</TableCell>
              <TableCell align="center">EditGroup</TableCell>
              <TableCell align="center">DeleteGroup</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {Array.isArray(data) && data.map((group) => (
              <TableRow key={group.id} sx={{ "&:last-child td, &:last-child th": { border: 0 } }}>
                <TableCell align="center">{group.title}</TableCell>
                <TableCell align="center">
                  {recipients.length > 0 ? getRecipientTitlesByIds(group.recipients, recipients) : "No Recipient"}
                </TableCell>
                <TableCell align="center">
                <InfoIcon aria-label="details" onClick={() => handleGroupDetailsModalOpen(group)} />
              </TableCell>
                <TableCell align="center">
                  <EditIcon aria-label="edit" onClick={() => handleEditModalOpen(group.id)} />
                </TableCell>
                <TableCell align="center">
                  <DeleteIcon aria-label="delete" onClick={() => handleDeleteModalOpen(group)} />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Button className="custom-btn add" onClick={handleCreateModalOpen}>
        <span>Add Group</span>
      </Button>

      {showGroupDetailsModal && selectedGroup  && (
        <GroupDetailsModal
          group={selectedGroup}
          recipients={recipients}
          open={Boolean(showGroupDetailsModal)}
          onClose={handleGroupDetailsModalClose}
        />
      )}

      {editModalGroupId && (
        <EditGroup
          show={Boolean(editModalGroupId)}
          onHide={handleEditModalClose}
          groupId={editModalGroupId}
        />
      )}

      {deleteModalGroup && (
        <DeleteGroup
          show={Boolean(deleteModalGroup)}
          onHide={handleDeleteModalClose}
          groupId={deleteModalGroup.id}
        />
      )}

      {showCreateModal && (
        <CreateGroup
          show={Boolean(showCreateModal)}
          onHide={handleCreateModalClose}
          onGroupCreated={handleCreateModalSave}
          availableRecipients={recipients}
        />
      )}
    </>
  );
}