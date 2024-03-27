import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/pagination';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  messages?: Message[] = [];
  pagination?: Pagination;
  container = 'Unread';
  loading = false;
  pageNumber = 0;
  pageSize = 5;

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(pageNumber = 0, pageSize = 5) {
    this.loading = true;
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.messageService
      .getMessages(this.pageNumber, this.pageSize, this.container)
      .subscribe({
        next: (response) => {
          this.messages = response.result;
          this.pagination = response.pagination;
          this.loading = false;
        },
      });
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe({
      next: (_) =>
        this.messages?.splice(
          this.messages.findIndex((m) => m.id === id),
          1
        ),
    });
  }

  pageChanged(event: any) {
    this.loadMessages(event.pageNumber, event.pageSize);
  }
}
