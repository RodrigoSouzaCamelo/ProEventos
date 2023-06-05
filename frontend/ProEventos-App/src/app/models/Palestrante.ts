import { RedeSocial } from "./RedeSocial";

export interface Palestrante {
  id: number;
  nome: string;
  miniCurriculo: string;
  imagemURL: string;
  telegone: string;
  email: string;
  redesSociais: RedeSocial[];
}
