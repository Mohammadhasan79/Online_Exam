import { useEffect, useState, useRef } from "react";
import { useNavigate, useParams } from "react-router-dom";

import Layout from "../components/Layout";
import examApi from "../api/examApi";
import answerApi from "../api/answerApi";

const questionTypes = {
    1: "تشریحی",
    2: "چهار گزینه‌ای",
    3: "جای خالی"
};
export default function StudentExamPage() {

    const { examId } = useParams();

    const navigate = useNavigate();

    const [loading, setLoading] = useState(true);

    const [answers, setAnswers] = useState([]);

    const [exam, setExam] = useState(null);

    const [remainingTime, setRemainingTime] = useState(0);

    const [examStarted, setExamStarted] = useState(false);

    const [examFinished, setExamFinished] = useState(false);

    const submittedRef = useRef(false);

    useEffect(() => {

        loadExam();

    }, []);
    useEffect(() => {

        if (!exam)
            return;

        const timer = setInterval(() => {

            calculateRemainingTime(exam);

        }, 1000);

        return () => clearInterval(timer);

    }, [exam]);


    async function loadExam() {

        try {

            const response = await examApi.getExamById(examId);
            console.log(response);

            setExam(response.data.data);

            calculateRemainingTime(response.data.data);

        }
        catch (error) {

            console.log(error);

        }
        finally {

            setLoading(false);

        }

    }

    function calculateRemainingTime(examData) {


        const start = new Date(examData.startTime+ "Z");

        const end = new Date(
            start.getTime() + examData.examTime * 60000
        );

        const now = new Date();

        if (now < start) {

            setExamStarted(false);

            setExamFinished(false);

            setRemainingTime(
                Math.floor((start - now) / 1000)
            );

            return;
        }

        if (now >= end) {

            setRemainingTime(0);

            setExamFinished(true);

            setExamStarted(false);

            autoSubmit();

            return;
        }

        setExamStarted(true);

        setExamFinished(false);

        setRemainingTime(
            Math.floor((end - now) / 1000)
        );

    }
    function handleAnswer(question, value) {

        setAnswers(prev => {

            const index = prev.findIndex(x => x.questionId === question.questionId);

            if (index !== -1) {

                const newAnswers = [...prev];

                newAnswers[index].userAnswer = value;

                return newAnswers;
            }

            return [

                ...prev,

                {
                    questionId: question.questionId,
                    questionType: question.questionType,
                    userAnswer: value
                }

            ];

        });

    }

    async function submitExam() {

        if (answers.length === 0) {

            alert("حداقل به یک سوال پاسخ دهید.");

            return;

        }

        try {

            await answerApi.submitAnswers(
                examId,
                answers
            );

            alert("آزمون با موفقیت ثبت شد.");

            navigate("/dashboard");

        }
        catch (error) {

            alert(
                error.response?.data?.message ??
                "خطا در ثبت آزمون"
            );

        }

    }
    async function autoSubmit() {
        if (submittedRef.current)
            return;

        submittedRef.current = true;

        try {

            await answerApi.submitAnswers(
                examId,
                answers
            );

            alert("زمان آزمون به پایان رسید. پاسخ‌ها به صورت خودکار ثبت شدند.");

            navigate("/dashboard");

        }
        catch (error) {

            console.log(error);

            submittedRef.current = false;

        }

    }
    if (loading) {

        return (
            <Layout>
                Loading...
            </Layout>
        );

    }
 
    function formatTime(seconds) {

        const h = Math.floor(seconds / 3600);

        const m = Math.floor((seconds % 3600) / 60);

        const s = seconds % 60;

        return `${String(h).padStart(2, "0")}:${String(m).padStart(2, "0")}:${String(s).padStart(2, "0")}`;

    }
    if (!examStarted && !examFinished) {

        return (

            <Layout>

                <div className="form-container">

                    <h2>

                        آزمون هنوز شروع نشده است.

                    </h2>

                    <p>

                        لطفاً در زمان تعیین شده وارد شوید.

                    </p>

                </div>

            </Layout>

        );

    }
    if (examFinished) {

        return (

            <Layout>

                <div className="form-container">

                    <h2>

                        زمان آزمون به پایان رسیده است.

                    </h2>

                </div>

            </Layout>

        );

    }

    return (

        <Layout>

            <div className="form-container">

                <h2>{exam.title}</h2>
                <div className="timer-box">

                    ⏰ زمان باقی مانده

                    <h2>

                        {formatTime(remainingTime)}

                    </h2>

                </div>
                <p>
                    تعداد سوالات: {exam.question?.length ?? 0}
                </p>

                <hr />

                {
                    exam.question.map((q, index) => (

                        <div
                            key={q.questionId}
                            className="question-card"
                        >

                           <h3>
                                سوال {index + 1}
                            </h3>

                            <p className="question-text">
                                {q.questionText}
                            </p>

                            {
                                q.questionType === 2 && (

                                    <div className="options">

                                        {
                                            q.options.map(option => (

                                                <label
                                                    key={option.optionKey}
                                                    className="option-card"
                                                >

                                                    <input
                                                        type="radio"
                                                        name={`question-${q.questionId}`}
                                                        value={option.optionKey}
                                                        checked={
                                                            answers.find(x => x.questionId === q.questionId)?.userAnswer === option.optionKey
                                                        }
                                                        onChange={(e) =>
                                                            handleAnswer(q, e.target.value)
                                                        }
                                                    />

                                                    {option.optionKey}) {option.option}

                                                </label>

                                            ))
                                        }

                                    </div>

                                )

                            }

                            {
                                q.questionType === 1 && (

                                    <textarea
                                        className="answer-input"
                                        placeholder="پاسخ خود را وارد کنید..."
                                        value={
                                            answers.find(x => x.questionId === q.questionId)?.userAnswer ?? ""
                                        }
                                        onChange={(e) =>
                                            handleAnswer(q, e.target.value)
                                        }
                                    />

                                )
                            }

                            {
                                q.questionType === 3 && (

                                    <input
                                        className="answer-input"
                                        placeholder="پاسخ را وارد کنید..."
                                        value={
                                            answers.find(x => x.questionId === q.questionId)?.userAnswer ?? ""
                                        }
                                        onChange={(e) =>
                                            handleAnswer(q, e.target.value)
                                        }
                                    />

                                )
                            }

                           

                            <p>

                                نوع سوال : { questionTypes[q.questionType]}

                            </p>

                        </div>

                    ))
                }
                <div
                    style={{
                        marginTop: "30px",
                        textAlign: "center"
                    }}
                >

                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={submitExam}
                    >
                        ثبت آزمون
                    </button>

                </div>
            </div>

        </Layout>

    );

}