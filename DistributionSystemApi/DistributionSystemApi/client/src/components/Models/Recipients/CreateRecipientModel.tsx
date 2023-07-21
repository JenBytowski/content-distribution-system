export default interface CreateRecipientModel {
    id: string;
    title: string;
    email: string;
    telephoneNumber? : string | null;
    groups :(string | undefined)[]
  }