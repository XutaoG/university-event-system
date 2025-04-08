import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import urlJoin from 'url-join';
import { environment } from '../../../environments/environment';
import {
	apiAddCommentRoute,
	apiDeleteCommentRoute,
	apiGetCommentsByEventRoute,
	apiUpdateCommentRoute,
} from '../../constants/api-routes';
import {
	AddCommentForm,
	CommentT,
	UpdateCommentForm,
} from '../../types/comment-types';

@Injectable({
	providedIn: 'root',
})
export class CommentService {
	private http = inject(HttpClient);

	getCommentByEvent(eventId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiGetCommentsByEventRoute,
			eventId.toString()
		);

		return this.http.get<CommentT[]>(url, { withCredentials: true });
	}

	addComment(comment: AddCommentForm, eventId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiAddCommentRoute,
			eventId.toString()
		);

		return this.http.post<CommentT>(url, comment, {
			withCredentials: true,
		});
	}

	editComment(comment: UpdateCommentForm, eventId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiUpdateCommentRoute,
			eventId.toString()
		);

		return this.http.put<CommentT>(url, comment, { withCredentials: true });
	}

	deleteComment(commentId: number) {
		const url = urlJoin(
			environment.apiUrl,
			apiDeleteCommentRoute,
			commentId.toString()
		);

		return this.http.delete<void>(url, { withCredentials: true });
	}
}
