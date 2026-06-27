// src/api/questionApi.js

import api from "./axiosInstance";

const questionApi = {
    createQuestion: (examId, data) =>
        api.post(`/api/exams/${examId}/Questions`, data),

    updateQuestion: (examId, questionId, data) =>
        api.put(`/api/exams/${examId}/Questions/${questionId}`, data),

    deleteQuestion: (examId, questionId) =>
        api.delete(`/api/exams/${examId}/Questions/${questionId}`)
};

export default questionApi;