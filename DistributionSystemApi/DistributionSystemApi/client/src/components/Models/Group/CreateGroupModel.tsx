export interface CreateGroupModel {
    id: string;
    title: string;
    recipientIds : (string | undefined)[];
  }