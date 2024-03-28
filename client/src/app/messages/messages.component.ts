import { DEFAULT_PAGE_SIZE } from './../_models/constants/paginationConstants';
import { Component, OnInit } from '@angular/core';
import { PageChangedEvent, Pagination } from '../_models/pagination';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';
import { DEFAULT_PAGE_NUMBER } from '../_models/constants/paginationConstants';

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
  pageNumber = DEFAULT_PAGE_NUMBER;
  pageSize = DEFAULT_PAGE_SIZE;

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(pageNumber = DEFAULT_PAGE_NUMBER, pageSize = DEFAULT_PAGE_SIZE) {
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
      complete: () => {
        if (this.pagination) {
          this.pagination.totalItems -= 1;
          if (this.pagination.totalItems % this.pageSize === 0) {
            //move page to previous page
            this.pagination.currentPage -= 1;
            this.pageNumber -= 1;
            this.loadMessages(this.pageNumber, this.pageSize);
          }
        }
      },
    });
  }

  pageChanged(event: PageChangedEvent) {
    this.loadMessages(event.pageNumber, event.pageSize);
  }
}
