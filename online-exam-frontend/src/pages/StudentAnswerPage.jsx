import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import Layout from "../components/Layout";
import answerApi from "../api/answerApi";

export default function StudentAnswerPage() {

    const { examId, studId } = useParams();

    const [loading, setLoading] = useState(true);

    const [result, setResult] = useState(null);

    useEffect(() => {

        loadAnswer();

    }, []);

    async function loadAnswer() {

        try {

            const response =
                await answerApi.getStudentAnswer(
                    examId,
                    studId
                );

            setResult(response.data.data);

        }
        catch (error) {

            console.log(error);

        }
        finally {

            setLoading(false);

        }

    }

    if (loading) {

        return (
            <Layout>
                Loading...
            </Layout>
        );

    }

    return (

        <Layout>

            <div className="form-container">

                <h2>

                    {result.examName}

                </h2>
                <p>
                    دانشجو:
                    {result.fistName} {result.lastName}
                </p>

                <p>
                    شماره دانشجویی:
                    {result.universityCode}
                </p>

                <hr />
                {
                    result.answers.map((answer, index) => (

                        <div
                            key={answer.answerId}
                            className="question-card"
                        >

                            <h3>

                                سوال {index + 1}

                            </h3>

                            <p className="question-text">

                                {answer.questionText}

                            </p>
                            {
                                answer.questionType === 2 && (

                                    <div className="options">

                                        {
                                            answer.options.map(option => (

                                                <div
                                                    key={option.optionKey}
                                                    className={
                                                        answer.userAnswer === option.optionKey
                                                            ? "selected-option"
                                                            : "normal-option"
                                                    }
                                                >

                                                    {option.optionKey}) {option.option}

                                                </div>

                                            ))
                                        }

                                    </div>

                                )
                            }
                            <p>

                                پاسخ دانشجو :

                                <strong>

                                    {answer.userAnswer ?? "بدون پاسخ"}

                                </strong>

                            </p>

                            {
                                answer.isCorrect !== null && (

                                    <p
                                        style={{
                                            color: answer.isCorrect ? "green" : "red",
                                            fontWeight: "bold"
                                        }}
                                    >

                                        {
                                            answer.isCorrect
                                                ? "✅ پاسخ صحیح"
                                                : "❌ پاسخ اشتباه"
                                        }

                                    </p>

                                )
                            }
                            <p>

                                پاسخ دانشجو:

                                <strong>

                                    {answer.userAnswer ?? "بدون پاسخ"}

                                </strong>

                            </p>

                        </div>

                    ))
                }
            </div>

        </Layout>

    );

}