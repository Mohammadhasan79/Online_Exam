import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";

import Layout from "../components/Layout";
import answerApi from "../api/answerApi";

export default function StudentAnswerListPage() {

    const { examId } = useParams();

    const [students, setStudents] = useState([]);

    const [loading, setLoading] = useState(true);

    useEffect(() => {

        loadStudents();

    }, []);

    async function loadStudents() {

        try {

            const response =
                await answerApi.getStudentsCompleteExamList(examId);

            setStudents(response.data.data);

        }
        catch (error) {

            console.log(error);

        }
        finally {

            setLoading(false);

        }

    }
    async function deleteAnswer(studId) {

        const ok = window.confirm(
            "آیا از حذف پاسخ این دانشجو مطمئن هستید؟"
        );

        if (!ok)
            return;

        try {

            await answerApi.deleteStudentAnswer(
                examId,
                studId
            );

            setStudents(prev =>
                prev.filter(x => x.studId !== studId)
            );

            alert("پاسخ حذف شد.");

        }
        catch (error) {

            alert(
                error.response?.data?.message ??
                "خطا در حذف پاسخ"
            );

        }
    }

    return (

        <Layout>

            <div className="form-container">

                <h2>دانشجویان شرکت کننده</h2>

                {
                    loading ?

                        <p>Loading...</p>

                        :

                        students.map(student => (

                            <div
                                key={student.studId}
                                className="question-card"
                            >

                                <h3>
                                    {student.firstName} {student.lastName}
                                </h3>

                                <p>{student.userName}</p>

                                <Link
                                    className="btn btn-primary"
                                    to={`/exams/${examId}/students/${student.studId}`}
                                >

                                    مشاهده پاسخ

                                </Link>
                                <button
                                    className="btn btn-danger"
                                    onClick={() => deleteAnswer(student.studId)}
                                >
                                    حذف پاسخ
                                </button>

                            </div>

                        ))
                }

            </div>

        </Layout>

    );

}