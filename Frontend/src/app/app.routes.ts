import { Routes } from '@angular/router';
import { App } from './app';
import { StudentComponent } from './student/student.component/student.component';
import { HomeComponent } from './home.component/home.component';
import { DepartmentComponent } from './department.component/department.component';
import { FacultyComponent } from './faculty.component/faculty.component';
import { ExamComponent } from './exam.component/exam.component';
import { QuestionComponent } from './question.component/question.component';
import { CourseComponent} from './course.component/course.component';
import { StudentAddComponent } from './student-CRUD/student-add/student-add';
import { StudentUpdateComponent } from './student-CRUD/student-update/student-update';
import { DepAddComponent } from './dep-CRUD/dep-add/dep-add';
import { DepUpdateComponent } from './dep-CRUD/dep-update/dep-update';
import { FacultyAddComponent } from './faculty-CRUD/faculty-add/faculty-add';
import { FacultyUpdateComponent } from './faculty-CRUD/faculty-update/faculty-update';
import { CourseAddComponent } from './course-CRUD/course-add/course-add';
import { CourseUpdateComponent } from './course-CRUD/course-update/course-update';
import { ExamAddComponent } from './exam-CRUD/exam-add/exam-add';
import { ExamUpdateComponent } from './exam-CRUD/exam-update/exam-update';
import { LoginComponent } from './login.component/login.component';
import { RegisterComponent } from './register.component/register.component';
import { GradeComponent } from './grade.component/grade.component';
import { AuthGuard } from './auth-guard';
import { RoleManagementComponent } from './role-management.component/role-management.component';
import { UnauthorizedComponent } from './unauthorized.component/unauthorized.component';
const routeConfig: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    {path:'login', component: LoginComponent},
    {path:'register', component: RegisterComponent},
    {path: 'students', component: StudentComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Student','Admin'] }
    },
    {path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
    {path: 'faculties', component: FacultyComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Faculty','Admin'] }
    },
    {path: 'departments', component: DepartmentComponent, 
        canActivate: [AuthGuard],
        data: { roles: ['Department','Admin'] }},
    {path: 'exams', component: ExamComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Exam','Admin','Student'] }
    },
    {path: 'exam/:id', component: QuestionComponent},
    {path:'courses', component:CourseComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Course','Admin'] }
    },
    {path:'student-add', component:StudentAddComponent},
    {path:'student-update/:id', component: StudentUpdateComponent, canActivate: [AuthGuard]},
    {path:'dep-add', component:DepAddComponent},
    {path:'dep-update/:id', component: DepUpdateComponent},
    {path:'faculty-add', component:FacultyAddComponent},
    {path:'faculty-update/:id', component: FacultyUpdateComponent},
    {path:'course-add', component:CourseAddComponent},
    {path:'course-update/:id', component: CourseUpdateComponent},
    {path:'exam-add', component:ExamAddComponent},
    {path:'exam-update/:id', component: ExamUpdateComponent},
    {path:'grades/:id', component: GradeComponent},
    {path:'roles', component:RoleManagementComponent},
    {path:'unauthorized', component:UnauthorizedComponent}
];

export default routeConfig;