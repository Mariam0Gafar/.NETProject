import { Question } from "./question";

export interface Exam{
    id: number,
    title: string,
    courseId: number | null,
    isActive: boolean
}