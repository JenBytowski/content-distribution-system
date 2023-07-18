export interface Recipient {
    id: string;
    title: string;
    email: string;
    telephoneNumber? : string;
    groups : string | null
  }