export interface UIState {
   showCreateProjectDialog: boolean;
   showEditProjectDialog: boolean;
   showCreateTicketDialog: boolean;
   showEditTicketDialog: boolean;
   editTicketId: string | null;
   showEditBoardDialog: boolean;
   editBoardId: string | null;
   showCreateBoardDialog: boolean;
}

export const INITIAL_STATE: UIState = {
   showCreateProjectDialog: false,
   showEditProjectDialog: false,
   showEditTicketDialog: false,
   editTicketId: null,
   showCreateTicketDialog: false,
   showCreateBoardDialog: false,
   showEditBoardDialog: false,
   editBoardId: null,
};
