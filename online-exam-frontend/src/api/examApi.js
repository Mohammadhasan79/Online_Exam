// src/api/examApi.js
import api from './axiosInstance';

export const examApi = {
    // Prof: لیست آزمون‌ها + لیست دانشجوها (برای صفحه تخصیص)
    getUserAndExamList: () => api.get('/api/Exams/GetUserAndExamList'),

    // Prof + Student: لیست آزمون‌های تخصیص داده شده
    getStudentAssignList: () => api.get('/api/Exams/GetStudentAssignList'),

    // Prof: تخصیص آزمون به دانشجو
    addStudentAssign: (studentId, examId) =>
        api.post('/api/Exams/AddStudentAssign', null, {
            params: { studentId, examId },
        }),

    // Prof: حذف تخصیص
    deleteStudentAssign: (studentAssignId) =>
        api.delete(`/api/Exams/DeleteStudentAssign/${studentAssignId}`),

    // Prof + Student: جزئیات یک آزمون با سوالاتش
    getExamById: (examId) => api.get(`/api/Exams/GetExamById/${examId}`),

    // Prof: لیست آزمون‌های خودش با جزئیات
    getExamByUserId: () => api.get('/api/Exams/GetExamByUserId'),

    // Prof: ساخت آزمون جدید
    createExam: (examDto) => api.post('/api/Exams/CreateExam', examDto),

    // Prof: ویرایش آزمون
    updateExam: (examId, examDto) =>
        api.put(`/api/Exams/CreateExam/${examId}`, examDto),

    // Prof: حذف آزمون
    deleteExam: (examId) =>
        api.delete('/api/Exams/DeleteExam', { params: { examId } }),
};

export default examApi;