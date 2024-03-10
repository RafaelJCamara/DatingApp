import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { MessageService } from 'src/app/_services/message.service';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonModule } from '@angular/material/button';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports: [
    CommonModule,
    TimeagoModule,
    FormsModule,
    MatTooltipModule,
    MatButtonModule,
  ],
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username?: string;
  messageContent = '';

  constructor(public messageService: MessageService) {}

  ngOnInit(): void {}

  sendMessage() {
    if (!this.username) return;
    this.messageService
      .sendMessage(this.username, this.messageContent)
      .then(() => {
        this.messageForm?.reset();
      });
  }

  computeMessageStatus(dateRead: Date | undefined, sendUsername: string) {
    if (this.isMessageUnread(dateRead, sendUsername)) return 'unread';

    if (this.isMessageRead(dateRead, sendUsername)) return 'read ';

    return;
  }

  isMessageUnread(dateRead: Date | undefined, sendUsername: string) {
    return !dateRead && sendUsername !== this.username;
  }

  isMessageRead(dateRead: Date | undefined, sendUsername: string) {
    return dateRead && sendUsername !== this.username;
  }
}
