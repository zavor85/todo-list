import React from 'react'
import '../App.css'

const ToDo = ({ todo, handleCheckBoxChange, remove }) => {
    return (
        <li className={todo.isCompleted === true ? "checked" : "todo"} >
            <input
                type="checkbox"
                name={todo.id}
                onChange={handleCheckBoxChange}
                checked={todo.isCompleted}
            />
            {todo.toDoTitle}
            <button onClick={remove}>x</button>
        </li>
    )
}

export default ToDo