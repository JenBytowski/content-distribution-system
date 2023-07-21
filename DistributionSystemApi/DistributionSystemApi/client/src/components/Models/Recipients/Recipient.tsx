import { Groups } from "../Group/Groups";

export default interface Recipient {
    id: string;
    title: string;
    email: string;
    telephoneNumber? : string | null;
    groups : Groups[]
  }