import { Groups } from "./Groups";

export interface Group {
  id: string;
  title: string;
  recipients: Groups[];
}