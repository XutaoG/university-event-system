import { User } from './auth-types';

export interface CommentT {
	commentID: number;
	uid: number;
	userResponse: User;
	eventID: number;
	text: string;
	rating: number;
	timestamp: Date;
}

export interface AddCommentForm {
	text: string;
	rating: number;
}

export interface UpdateCommentForm {
	text: string;
	rating: number;
}
