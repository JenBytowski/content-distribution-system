import { Recipient } from "../Recipients/Recipient";

export interface Group {
    id: string;
    title: string;
    recipients: Recipient[];
  };