import React, { useState, useEffect } from 'react';
import axios from 'axios';

function App() {
    const [books, setBooks] = useState([]);
    const [newBook, setNewBook] = useState({ title: '', author: '', copiesAvailable: 1 });

    useEffect(() => {
        fetchBooks();
    }, []);

    const fetchBooks = async () => {
        try {
          
            const response = await axios.get('https://localhost:7128/api/LibraryManagment/GetBooks');
            setBooks(response.data);
        } catch (error) {
            console.error('Error fetching books:', error);
        }
    };

    const addBook = async () => {
        try {
            await axios.post('https://localhost:7128/api/LibraryManagment/AddBook', newBook);
            fetchBooks();
            setNewBook({ title: '', author: '', copiesAvailable: 1 });
        } catch (error) {
            console.error('Error adding book:', error);
        }
    };

    const deleteBook = async (id) => {
        try {
            await axios.delete(`https://localhost:7128/api/LibraryManagment/DeleteBook/${id}`);
            fetchBooks();
        } catch (error) {
            console.error('Error deleting book:', error);
        }
    };

    const borrowBook = async (id) => {
        try {
            await axios.post(`https://localhost:7128/api/LibraryManagment/LendBook/${id}/lend`);
            fetchBooks();
        } catch (error) {
            console.error('Error borrowing book:', error);
        }
    };

    const returnBook = async (id) => {
        try {
            await axios.post(`https://localhost:7128/api/LibraryManagment/ReturnBook/${id}/return`);
            fetchBooks();
        } catch (error) {
            console.error('Error returning book:', error);
        }
    };

    return (
        <div className="App">
            <h1>Library Management</h1>
            <div>
                <input
                    type="text"
                    placeholder="Title"
                    value={newBook.title}
                    onChange={(e) => setNewBook({ ...newBook, title: e.target.value })}
                />
                <input
                    type="text"
                    placeholder="Author"
                    value={newBook.author}
                    onChange={(e) => setNewBook({ ...newBook, author: e.target.value })}
                />
                <input
                    type="number"
                    placeholder="Copies Available"
                    value={newBook.copiesAvailable}
                    onChange={(e) => setNewBook({ ...newBook, copiesAvailable: parseInt(e.target.value) })}
                />
                <button onClick={addBook}>Add Book</button>
            </div>
            <h2>Books List</h2>
            <ul>
                {books.map(book => (
                    <li key={book.id}>
                        {book.title} by {book.author} - Copies Available: {book.copiesAvailable}
                        <button onClick={() => borrowBook(book.id)}>Borrow</button>
                        <button onClick={() => returnBook(book.id)}>Return</button>
                        <button onClick={() => deleteBook(book.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default App;
