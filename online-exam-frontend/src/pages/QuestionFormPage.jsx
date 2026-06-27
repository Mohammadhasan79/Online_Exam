import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Layout from "../components/Layout";
import examApi from "../api/examApi";
import questionApi from "../api/questionApi";

export default function QuestionFormPage() {

    const navigate = useNavigate();

    const { examId, questionId } = useParams();

    const isEdit = !!questionId;

    const [loading, setLoading] = useState(isEdit);

    const [form, setForm] = useState({

        questionText: "",

        questionType: 2,

        imageUrl: "",

        currectAnswer: "A",

        options: [
            { optionKey: "A", option: "" },
            { optionKey: "B", option: "" },
            { optionKey: "C", option: "" },
            { optionKey: "D", option: "" }
        ]

    });

    useEffect(() => {

        if (isEdit) {

            loadQuestion();

        }

    }, []);

    async function loadQuestion() {

        const response = await examApi.getExamById(examId);

        const question = response.data.data.question.find(x => x.questionId == questionId);

        setForm({

            questionText: question.questionText,

            questionType: question.questionType,

            imageUrl: question.imageUrl ?? "",

            currectAnswer: question.currectAnswer,

            options: question.options

        });

        setLoading(false);

    }
    function handleChange(e) {

        const { name, value } = e.target;

        setForm(prev => ({
            ...prev,
            [name]: name === "questionType" ? Number(value) : value
        }));

    }
    function handleOptionChange(index, value) {

        const newOptions = [...form.options];

        newOptions[index].option = value;

        setForm(prev => ({
            ...prev,
            options: newOptions
        }));

    }
    async function handleSubmit(e) {

        e.preventDefault();

        try {
            const data = {
                ...form,
                options: form.questionType === 2 ? form.options : [],
                currectAnswer: form.questionType === 2 ? form.currectAnswer : null
            };
            if (isEdit) {

                await questionApi.updateQuestion(
                    examId,
                    questionId,
                    data
                );

            } else {

                await questionApi.createQuestion(
                    examId,
                    data
                );

            }

            navigate(`/exams/${examId}/questions`);

        }
        catch (error) {

            alert(error.response?.data?.message ?? "خطا در ذخیره سوال");

        }

    }

    if (loading) {

        return (
            <Layout>

                Loading...

            </Layout>
        )

    }

    return (

        <Layout>
            <div className="form-container">
            <h2>

                {isEdit ? "ویرایش سوال" : "افزودن سوال"}

            </h2>

                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                <label>متن سوال</label>

                <textarea
                    name="questionText"
                    value={form.questionText}
                    onChange={handleChange}
                        />
                    </div>
                    <div className="form-group">
                <label>نوع سوال</label>

                <select
                    name="questionType"
                    value={form.questionType}
                    onChange={handleChange}
                >

                    <option value={1}>تشریحی</option>

                    <option value={2}>چهار گزینه ای</option>

                    <option value={3}>جای خالی</option>

                        </select>
                    </div>
                    <div className="form-group">
                <label>

                    تصویر (اختیاری)

                </label>

                <input
                    name="imageUrl"
                    value={form.imageUrl}
                    onChange={handleChange}
                        />
                    </div>
                {
                    form.questionType === 2 && (

                        <>

                            {
                                form.options.map((option, index) => (

                                    
                                        <div key={option.optionKey}>
                                        <label>

                                            گزینه {option.optionKey}

                                        </label>

                                        <input 
                                            type="text"
                                            className="question-option-input"
                                            value={option.option}
                                            onChange={(e) =>
                                                handleOptionChange(index, e.target.value)
                                            }
                                        />

                                    </div>

                                ))
                            }
                            <div className="form-group">
                            <label>

                                پاسخ صحیح

                            </label>

                            <select
                                name="currectAnswer"
                                value={form.currectAnswer}
                                onChange={handleChange}
                            >

                                <option>A</option>

                                <option>B</option>

                                <option>C</option>

                                <option>D</option>

                                    </select>
                                </div>
                        </>
                       
                    )
                }
                    <button
                        type="submit"
                        className="save-btn"
                    >

                        {isEdit ? "ویرایش سوال" : "ثبت سوال"}

                    </button>
                </form>
            </div>
        </Layout>

    )

}