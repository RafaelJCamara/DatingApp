import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/pagination';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  messages?: Message[] = [];
  pagination?: Pagination;
  container = 'Unread';
  pageNumber = 0;
  pageSize = 5;
  loading = false;
  pageSizeArray = [5, 10, 25];

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.loading = true;
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

  pageChanged(event: PageEvent) {
    let changes = false;

    if (this.pageNumber != event.pageIndex) {
      this.pageNumber = event.pageIndex;
      changes = true;
    }

    if (this.pageSize != event.pageSize) {
      this.pageSize = event.pageSize;
      changes = true;
    }

    if (changes) this.loadMessages();
  }
}
