import { Component, inject, OnInit } from '@angular/core';
import { ModalService } from '../../../services/modal/modal.service';
import { EventT, RsoEvent } from '../../../types/event-types';
import { parseDate, parseTime } from '../../../utils';
import { RsoService } from '../../../services/rso/rso.service';
import { CommentService } from '../../../services/comment/comment.service';
import { ListContainerComponent } from '../../list-container/list-container.component';
import {
	AddCommentForm,
	CommentT,
	UpdateCommentForm,
} from '../../../types/comment-types';
import { CommentComponent } from '../../comment/comment.component';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth/auth.service';
import { User } from '../../../types/auth-types';

@Component({
	selector: 'app-event-modal-view',
	imports: [ListContainerComponent, CommentComponent, FormsModule],
	templateUrl: './event-modal-view.component.html',
	styleUrls: ['./event-modal-view.component.scss', '../modal.component.scss'],
})
export class EventModalViewComponent implements OnInit {
	private modalService = inject(ModalService);
	private rsoService = inject(RsoService);
	private commentService = inject(CommentService);
	private authService = inject(AuthService);

	user: User | null = null;
	event: EventT | null = null;

	eventDateParsed = '';
	eventTimeParsed = '';
	eventTimeEndParsed = '';

	rsoName: string | null = null;

	comments: CommentT[] = [];

	addCommentText: string = '';
	addCommentRating: number = 5;

	ngOnInit() {
		// Subscribe to event behaviorSubject
		this.modalService.event$.subscribe((event) => {
			this.event = event;

			if (event != null) {
				this.eventDateParsed = parseDate(event.eventDate);
				this.eventTimeParsed = parseTime(event.eventTime);
				this.eventTimeEndParsed = parseTime(event.eventTimeEnd);

				// Get RSO Name
				const rsoId = (event as RsoEvent).rsoid;
				if (rsoId != null) {
					// Set RSO Name
					this.rsoService.getRsoById(rsoId).subscribe((rso) => {
						return (this.rsoName = rso.name);
					});
				} else {
					this.rsoName = null;
				}

				// Get Comments
				this.getComments();
			}
		});

		// Subscribe to user behaviorSubject
		this.authService.user$.subscribe((user) => {
			this.user = user;
		});
	}

	getComments() {
		if (this.event == null) {
			return;
		}

		this.commentService
			.getCommentByEvent(this.event.eventID)
			.subscribe((comments) => {
				this.comments = comments;
			});
	}

	onClose() {
		this.modalService.setEvent(null);
		this.modalService.setRso(null);
	}

	addComment() {
		const comment: AddCommentForm = {
			text: this.addCommentText.trim(),
			rating: this.addCommentRating,
		};

		this.commentService
			.addComment(comment, this.event!.eventID)
			.subscribe(() => {
				this.getComments();
				this.addCommentText = '';
			});
	}

	updateComment(updatedComment: {
		commentId: number;
		comment: UpdateCommentForm;
	}) {
		this.commentService
			.editComment(updatedComment.comment, updatedComment.commentId)
			.subscribe(() => {
				this.getComments();
			});
	}

	deleteComment(commentId: number) {
		this.commentService.deleteComment(commentId).subscribe(() => {
			this.getComments();
		});
	}
}
