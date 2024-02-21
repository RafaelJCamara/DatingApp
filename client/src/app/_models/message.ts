export interface Message {
  createHubConnection(user: any, userName: string): unknown;
  stopHubConnection(): unknown;
  getMessageThread(userName: string): unknown;
  id: number;
  senderId: number;
  senderUsername: string;
  senderPhotoUrl: string;
  recipientId: number;
  recipientUsername: string;
  recipientPhotoUrl: string;
  content: string;
  dateRead?: Date;
  messageSent: Date;
}
