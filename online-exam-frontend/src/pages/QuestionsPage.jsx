import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import Layout from "../components/Layout";
import examApi from "../api/examApi";
import questionApi from "../api/questionApi";

export default function QuestionsPage() {

    const { examId } = useParams();

    const [exam, setExam] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadExam();
    }, []);

    async function loadExam() {

        try {

            const response = await examApi.getExamById(examId);

            setExam(response.data.data);

        }
        finally {

            setLoading(false);

        }

    }
    async function deleteQuestion(questionId) {

        const confirmDelete = window.confirm(
            "آیا از حذف این سوال مطمئن هستید؟"
        );

        if (!confirmDelete) return;

        try {

            await questionApi.deleteQuestion(
                examId,
                questionId
            );

            await loadExam();

        }
        catch (error) {

            alert(error.response?.data?.message ?? "خطا در حذف سوال");

        }

    }

    if (loading)
        return (
            <Layout>
                <h2>در حال بارگذاری...</h2>
            </Layout>
        );

    return (

        <Layout>

            <div className="page-header">

                <div>

                    <h2>{exam.examName}</h2>

                    <p>{exam.examDescription}</p>

                </div>

                <Link
                    className="btn-primary"
                    to={`/exams/${examId}/questions/new`}
                >
                    + افزودن سوال
                </Link>

            </div>

            {
                exam.question.length === 0 &&

                <div className="empty-box">

                    هنوز سوالی ثبت نشده است.

                </div>

            }

            {
                exam.question.map((q, index) => (

                    <div className="question-card" key={q.questionId}>

                        <div className="question-top">

                            <h3>

                                سوال {index + 1}

                            </h3>

                            <span>

                                {getQuestionType(q.questionType)}

                            </span>

                        </div>

                        <p>

                            {q.questionText}

                        </p>

                        {
                            q.questionType === 2 &&
                            q.options.length > 0 && (

                                <div className="options">

                                    {
                                        q.options.map(option => (

                                            <div key={option.optionId}>

                                                <b>{option.optionKey})</b> {option.option}

                                            </div>

                                        ))
                                    }

                                </div>

                            )
                        }

                        <div className="question-actions">

                            <Link
                                to={`/exams/${examId}/questions/${q.questionId}/edit`}
                            >
                                ویرایش
                            </Link>

                            <button
                                onClick={() => deleteQuestion(q.questionId)}
                            >
                                حذف
                            </button>

                        </div>

                    </div>

                ))
            }

        </Layout>

    );

}

function getQuestionType(type) {

    switch (type) {

        case 1:
            return "تشریحی";

        case 2:
            return "چهار گزینه ای";

        case 3:
            return "جای خالی";

        default:
            return "";

    }

}