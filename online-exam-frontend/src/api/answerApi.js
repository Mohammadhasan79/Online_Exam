import axiosInstance from "./axiosInstance";

const answerApi = {

    submitAnswers(examId, answers) {

        return axiosInstance.post(
            `/api/exams/${examId}/Answers/SubmitAnswers`,
            answers
        );

    },

    getStudentsCompleteExamList(examId) {

        return axiosInstance.get(
            `/api/exams/${examId}/Answers/GetStudentsCompeletExamList`
        );

    },

    getStudentAnswer(examId, studId) {

        return axiosInstance.get(
            `/api/exams/${examId}/Answers/GetStudentAnswer`,
            {
                params: {
                    studId
                }
            }
        );

    },
    deleteStudentAnswer(examId, studId) {
        return axiosInstance.delete(
            `/api/exams/${examId}/Answers/DeleteStudentAnswer`,
            {
                params: {
                    studId
                }
            }
        );
    }

};

export default answerApi;