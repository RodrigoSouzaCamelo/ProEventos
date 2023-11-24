import { Evento } from "./Evento";
import { RedeSocial } from "./RedeSocial";
import { UserUpdate } from "./UserUpdate";

export interface Palestrante {
  id: number;
  miniCurriculo: string;
  user: UserUpdate;
  eventos: Evento[];
  redesSociais: RedeSocial[];
}
