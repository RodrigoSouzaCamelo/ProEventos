import { Lote } from "./Lote";
import { RedeSocial } from "./RedeSocial";

export interface Evento {
  id: number;
  local: string;
  data?: Date;
  tema: string;
  qtdPessoas: number;
  imagemURL: string;
  telefone: string;
  email: string;
  lotes: Lote[];
  redesSociais: RedeSocial[];
}
