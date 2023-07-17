export default interface Recipient {
    id: string;
    title: string;
    email: string;
    telephoneNumber? : string;
    groupId : string | null
  }