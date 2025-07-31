import { Injectable } from '@angular/core';
import { Exam } from './exam';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExamSubmission } from './ExamSubmission';
import { ExamSubmissionResult } from './ExamSubmissionResult';
import { StudentExam } from './StudentExam';
@Injectable({
  providedIn: 'root'
})
export class ExamService {
  private apiUrl = 'https://localhost:7096/Exam';

  constructor(private http: HttpClient) { }

  getExams(): Observable<Exam[]>{
    return this.http.get<Exam[]>(this.apiUrl);
  }

  getExamWithQuestions(id: number): Observable<Exam> {
  const url = `${this.apiUrl}/${id}`;
  return this.http.get<Exam>(url);
}
deleteExam(id: number): Observable<Exam> {
    debugger
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete<Exam>(url);
  }

  getExamById(id: number): Observable<Exam> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Exam>(url);
  }

  getExamsPaged(page: number = 1, pageSize: number = 5, sortOrder: string = 'asc',name?: string)
    : Observable<{exams: Exam[],totalPages: number,totalCount: number,page: number}> {
      let params = `?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`;
      if (name) {
        params += `&name=${encodeURIComponent(name)}`;
      }
      return this.http.get<{
        exams: Exam[],
        totalPages: number,
        totalCount: number,
        page: number
      }>(`${this.apiUrl}/paged${params}`);
    }

    submitExam(e: ExamSubmission) : Observable<ExamSubmissionResult>{
      const url = 'https://localhost:7096/StudentExam/Submit';
      return this.http.post<ExamSubmissionResult>(url, e);
    }

  prepareSubmission(examId: number, studentId: number): Observable<ExamSubmission> {
    const url = `https://localhost:7096/StudentExam/prepareSubmission?examId=${examId}&studentId=${studentId}`;
    return this.http.get<ExamSubmission>(url);
}
   addExam(f: Exam): Observable<Exam> {
        return this.http.post<Exam>(this.apiUrl, f);
      }
    addQuestion(question: any): Observable<any> {
      return this.http.post('https://localhost:7096/Question', question); 
    }

    updateExam(exam: Exam): Observable<any> {
    return this.http.put(this.apiUrl, exam);
}
    updateQuestion(question: any): Observable<any>{
      return this.http.put('https://localhost:7096/Question',question);
    }

    getQuestionsByExamId(id: number): Observable<any>{
      return this.http.get(`https://localhost:7096/Question/Exam?id=${id}`);
    }

    getGradesForStudent(sId: number): Observable<any>{
      return this.http.get(`https://localhost:7096/StudentExam/GetGradesForStudent?studentId=${sId}`)
    }

getStudentExams(sortOrder: string = 'asc'): Observable<StudentExam[]> {
  let params = `?sortOrder=${sortOrder}`;
  return this.http.get<StudentExam[]>(`https://localhost:7096/StudentExam/All?${params}`);
}

}
