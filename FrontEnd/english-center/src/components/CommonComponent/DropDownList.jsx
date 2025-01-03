import React, { useEffect, useState } from 'react'
import "./DropDownList.css";

function DropDownList({ data, defaultIndex, onSelectedItem, placeholder, className, tblClassName,name, hideDefault, readOnly = false }) {
    const [isShowList, setIsShowList] = useState(false);
    const [selectedIndex, setSelectedIndex] = useState(defaultIndex);
    const [inputValue, setInputValue] = useState(data[defaultIndex]?.key ?? "");

    useEffect(() => {
        setSelectedIndex(selectedIndex);
        setInputValue(data[selectedIndex]?.key ?? "");
    }, [data])

    useEffect(() => {
        setSelectedIndex(defaultIndex);
        setInputValue(data[defaultIndex]?.key ?? "");

        if (onSelectedItem) {
            onSelectedItem(data[defaultIndex], defaultIndex);
        }
    }, [defaultIndex])

    const handleClickDDL = () => {
        if (readOnly == false) {
            setIsShowList(!isShowList);
        }
    }

    const handleSetValue = (item, index) => {
        if (index == -1) {
            setInputValue("");
            setIsShowList(false);
            setSelectedIndex(-1);
            onSelectedItem(null, -1);
        }
        else {
            setInputValue(item.key);
            setIsShowList(false);
            setSelectedIndex(index);
            onSelectedItem(item, index);
        }
    }

    return (
        <div className={`drop-down-list__wrapper relative overflow-visible`}>
            <input
                type="text"
                placeholder={placeholder}
                name={name}
                value={inputValue}
                readOnly={true}
                className={`cursor-pointer ddl--input w-full ${className}`}
                onClick={handleClickDDL}
            />

            <div className={`${tblClassName} ddl-list absolute z-[100] w-full top-[95%] left-0 h-[256px] overflow-scroll  mt-[10px] transition-all duration-200 ease-out ${isShowList ? "max-h-fit" : "max-h-0 pt-0"}`}>

                {hideDefault == false &&
                    <div
                        className={`ddl--item ${selectedIndex == -1 && "selected"}`}
                        onClick={(e) => handleSetValue("", -1)}
                    >
                        Default
                    </div>
                }

                {data.map((item, index) => {
                    return (
                        <div
                            className={`ddl--item ${selectedIndex == index && "selected"}`}
                            onClick={(e) => handleSetValue(item, index)}
                            key={index}
                        >
                            {item.key}
                        </div>
                    )
                })}
            </div>
        </div>
    );
}

export default DropDownList