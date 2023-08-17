import {
   ProjectDetail,
   ProjectPostData,
   ProjectUpdate,
   TicketUpdate,
   ProjectBrief,
   SortDirection,
   SortDirectionType,
   TicketBrief,
   TicketStatusType,
   BoardDetail,
   TicketPostData,
   TicketDetail,
   BoardPostData,
   BoardUpdate
} from "./types";
import axios from "axios";

const API_BASE_URL = "https://localhost:7093";

// Returned in response to GET requests for single records
export interface GetResult<T> {
   success: boolean;
   data?: T;
}

// Returned in response to GET requests to list multiple records
export interface ListResult<Record> {
   success: boolean;
   items: Record[];
   totalRecords: number;
}

export interface PostResult {
   success: boolean;
   createdId: string;
}

export interface PutResult {
   success: boolean;
}

export interface QueryParameters {
   searchQuery?: string;
   offset?: number;
   limit?: number;
   sortBy?: string;
   sortDirection?: SortDirectionType;
   filters?: Array<{
      key: string;
      value: string;
   }>;
}

export const queryTickets = async (
   qParams: QueryParameters
): Promise<ListResult<TicketBrief>> => {
   let urlParams = "?";
   if (qParams.searchQuery)
   {
      urlParams += `search=${qParams.searchQuery}&`;
   }
   if (qParams.offset) {
      urlParams += `offset=${qParams.offset}&`;
   }
   if (qParams.limit) {
      urlParams += `limit=${qParams.limit}&`;
   }
   if (qParams.sortBy) {
      urlParams += `orderBy=${qParams.sortBy}&`;
   }
   if (qParams.sortDirection) {
      urlParams += `orderAscending=${
         qParams.sortDirection === SortDirection.Ascending
      }&`;
   }
   if (qParams.filters) {
      qParams.filters.forEach(({ key, value }) => {
         urlParams += `${key}=${value}&`;
      });
   }
   if (urlParams.endsWith("&"))
   {
      urlParams = urlParams.slice(0, urlParams.length-1);
   }

   const result = await axios.get(`${API_BASE_URL}/api/tickets${urlParams}`);

   return {
      success: result.status === 200,
      items: result.data.items,
      totalRecords: result.data.totalRecords,
   };
};

const putRecord = async (
   resource: string,
   resourceId: string,
   data: any
): Promise<PutResult> => {
   const response = await axios.put(
      `${API_BASE_URL}${resource}/${resourceId}`,
      data
   );
   return {
      success: response.status === 200,
   };
};

export const putTicket = async (
   ticketId: string,
   updatedName: string,
   updatedDescription: string,
   updatedStatus: TicketStatusType
): Promise<PutResult> => {
   const data: TicketUpdate = {
      name: updatedName,
      description: updatedDescription,
      status: updatedStatus,
   };
   return putRecord("/api/tickets", ticketId, data);
};

export const putProject = async (
   projectId: string,
   updatedName: string,
   updatedColorCode: string
) => {
   const data: ProjectUpdate = {
      name: updatedName,
      hexColorCode: updatedColorCode,
   };
   return putRecord("/api/projects", projectId, data);
};

export const putBoard = async (
   boardId: string,
   boardUpdate: BoardUpdate
) => {
   return putRecord("/api/boards", boardId, boardUpdate);
};

const postRecord = async (resource: string, data: any): Promise<PostResult> => {
   const response = await axios.post(`${API_BASE_URL}${resource}`, data);
   return {
      success: response.status === 200,
      createdId: response.data.createdId,
   };
};

export const postProject = async (
   name: string,
   hexColorCode: string
): Promise<PostResult> => {
   const data: ProjectPostData = {
      name,
      hexColorCode,
   };
   return postRecord("/api/projects", data);
};

export const postTicket = async (
   name: string,
   description: string,
   status: TicketStatusType,
   projectId: string,
): Promise<PostResult> => {
   const data: TicketPostData = {
      name,
      description,
      status,
      projectId
   };
   return postRecord("/api/tickets", data);
};

export const postBoard = async (
   name: string,
   projectId: string,
   columns: Array<{
      name: string;
      index: number;
   }>
): Promise<PostResult> => {
   const data: BoardPostData = {
      name,
      projectId,
      columns
   };
   return postRecord("/api/boards", data);
};

const getRecord = async <Record>(
   resource: string,
   resourceId: string
): Promise<GetResult<Record>> => {
   const response = await axios.get(
      `${API_BASE_URL}${resource}/${resourceId}`
   );
   return {
      success: response.status === 200,
      data: response.data,
   };
};

export const getProject = async (
   projectId: string
): Promise<GetResult<ProjectDetail>> => {
   return getRecord("/api/projects", projectId);
};

export const getBoard = async (
   boardId: string
): Promise<GetResult<BoardDetail>> => {
   return getRecord("/api/boards", boardId);
};

export const getTicket = async (
   ticketId: string
): Promise<GetResult<TicketDetail>> => {
   return getRecord("/api/tickets", ticketId);
};

export const listProjects = async (): Promise<ListResult<ProjectBrief>> => {
   const response = await axios.get(`${API_BASE_URL}/api/projects`);
   return {
      success: response.status === 200,
      items: response.data.items,
      totalRecords: response.data.totalRecords,
   };
};
