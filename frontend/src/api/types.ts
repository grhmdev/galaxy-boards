/** Inbound data from server **************************************/

export interface ProjectBrief {
   id: string;
   name: string;
   detail: string;
   hexColorCode: string;
   links: {[key: string]: string}
}

export interface ProjectDetail extends ProjectBrief {
   tickets: TicketBrief[];
   boards: BoardBrief[];
}

export interface BoardBrief {
   id: string;
   name: string;
   links: {[key: string]: string}
}

export interface BoardDetail extends BoardBrief {
    project: ProjectBrief;
    columns: BoardColumn[];
}

export interface BoardColumn {
    id: string;
    name: string;
    index: number;
    ticketPlacements: TicketPlacement[];
}

export interface TicketPlacement {
   id: string;
   ticketId: string;
   boardColumnId: string;
   index: number;
}

export type TicketStatusType = "Open" | "Closed";
export const TicketStatus: { [key: string]: TicketStatusType } = {
   Closed: "Closed",
   Open: "Open",
};

export interface TicketBrief {
   id: string;
   name: string;
   status: TicketStatusType;
   links: {[key: string]: string}
}

export interface TicketDetail extends TicketBrief {
   description: string;
}

/** Outbound data to server **************************************/

export type SortDirectionType = "ascending" | "descending";
export const SortDirection: { [key: string]: SortDirectionType } = {
   Ascending: "ascending",
   Descending: "descending",
};

export interface ProjectUpdate {
   name: string;
   hexColorCode: string;
}

export interface ProjectPostData {
   name: string;
   hexColorCode: string;
}

export interface TicketUpdate {
   name: string;
   status: string;
   description: string;
}

export interface TicketPostData {
   name: string;
   status: string;
   description: string;
   projectId: string;
}

export interface BoardPostData {
   name: string;
   projectId: string;
   columns: Array<{
      name: string;
      index: number;
   }>
}

export interface TicketPlacementUpdate {
   id: string | null;
   ticketId: string;
   index: number;
}

export interface BoardColumnUpdate {
   id: string | null;
   name: string;
   index: number;
   ticketPlacements: TicketPlacementUpdate[];
}

export interface BoardUpdate {
   name: string;
   columns: BoardColumnUpdate[];
}