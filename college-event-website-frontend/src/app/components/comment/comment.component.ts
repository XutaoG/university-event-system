import { Component, input, OnInit, output } from '@angular/core';
import { CommentT, UpdateCommentForm } from '../../types/comment-types';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';

@Component({
	selector: 'app-comment',
	imports: [MatIconModule, FormsModule],
	templateUrl: './comment.component.html',
	styleUrl: './comment.component.scss',
})
export class CommentComponent implements OnInit {
	comment = input.required<CommentT>();
	own = input.required<boolean>();

	commentId = output<number>();

	isEditing = false;
	editCommentText = '';
	editCommentRating = 0;
	updatedComment = output<{
		commentId: number;
		comment: UpdateCommentForm;
	}>();

	ngOnInit() {
		this.editCommentText = this.comment().text;
		this.editCommentRating = this.comment().rating;
	}

	onEditToggle() {
		this.isEditing = true;
	}

	onEditCancel() {
		this.isEditing = false;
		this.editCommentText = this.comment().text;
		this.editCommentRating = this.comment().rating;
	}

	onEdit() {
		const updatedComment: UpdateCommentForm = {
			text: this.editCommentText,
			rating: this.editCommentRating,
		};
		this.updatedComment.emit({
			commentId: this.comment().commentID,
			comment: updatedComment,
		});

		this.isEditing = false;
	}

	onDelete() {
		this.commentId.emit(this.comment().commentID);
	}
}
