import { StudentAnswer } from "./StudentAnswer"
export interface ExamSubmission{
    title: string,
    studentId: number,
    examId: number,
    studentAnswers: StudentAnswer[]
}