function highlightByWords(text, highlights) {
    return text.split(" ").map((word, idx) => {
        const highlight = highlights.find(h => word.includes(h.word));
        if (highlight) {
            return (
                <span key={idx} className={highlight.className}>
                    {word + " "}
                </span>
            );
        }
        return word + " ";
    });
}

export default highlightByWords;